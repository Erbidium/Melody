import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { columnsToDisplayWithFavouriteColumn } from '@core/helpers/columns-to-display-helper';
import { ISongFromPlaylist } from '@core/models/ISongFromPlaylist';
import { InfiniteScrollingService } from '@core/services/infinite-scrolling.service';
import { NotificationService } from '@core/services/notification.service';
import { PlayerService } from '@core/services/player.service';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { UserService } from '@core/services/user.service';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-recommendations-page',
    templateUrl: './recommendations-page.component.html',
    styleUrls: ['./recommendations-page.component.sass'],
})
export class RecommendationsPageComponent extends BaseComponent implements OnInit {
    recommendedSongs: ISongFromPlaylist[] = [];

    columnsToDisplay = columnsToDisplayWithFavouriteColumn;

    currentSongIdForMusicPlayer?: number;

    page = 1;

    pageSize = 10;

    userFilledPreferences = false;

    constructor(
        private songService: SongService,
        private userService: UserService,
        private spinnerOverlayService: SpinnerOverlayService,
        private playerService: PlayerService,
        private scrollService: InfiniteScrollingService,
        private notificationService: NotificationService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadRecommendedSongs(this.page, this.pageSize);
        this.playerService.playerStateEmitted$.pipe(this.untilThis).subscribe((state) => {
            this.currentSongIdForMusicPlayer = state.id;
        });

        this.scrollService
            .getObservable()
            .pipe(this.untilThis)
            .subscribe((status: { isIntersecting: boolean; id: string }) => {
                if (status.isIntersecting && status.id === `target${this.page * this.pageSize - 1}`) {
                    this.loadRecommendedSongs(++this.page, this.pageSize);
                }
            });
    }

    selectSong(songId: number) {
        this.playerService.emitPlayerStateChange(songId, this.recommendedSongs);
    }

    loadRecommendedSongs(page: number, pageSize: number, updateAllData = false) {
        this.spinnerOverlayService.show();
        const recommendedSongs = this.songService.getRecommendedSongs(page, pageSize);
        const userPreferencesStatus = this.userService.checkUserRecommendationPreferences();

        forkJoin([recommendedSongs, userPreferencesStatus])
            .pipe(this.untilThis)
            .subscribe({
                next: (resp) => {
                    this.spinnerOverlayService.hide();

                    this.recommendedSongs = updateAllData ? resp[0] : this.recommendedSongs.concat(resp[0]);
                    [, this.userFilledPreferences] = resp;

                    const clear = setInterval(() => {
                        const target = document.querySelector(`#target${page * pageSize - 1}`);

                        if (target) {
                            clearInterval(clear);
                            this.scrollService.setObserver().observe(target);
                        }
                    }, 100);
                },
                error: (e) => {
                    if (e instanceof HttpErrorResponse && e.status === 404) {
                        this.notificationService.showWarningMessage(
                            'Не вдалося отримати рекомендації. Не заповнено вподобання',
                        );
                        this.spinnerOverlayService.hide();
                    } else {
                        this.notificationService.showErrorMessage('Трапилася помилка');
                    }
                },
            });
    }

    changeSongStatus(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.spinnerOverlayService.show();
        const song = this.recommendedSongs.find((s) => s.id === id);

        if (song) {
            this.songService
                .setSongStatus(id, !song.isFavourite)
                .pipe(
                    switchMap(() =>
                        forkJoin([
                            this.songService.getRecommendedSongs(1, this.page * this.pageSize),
                            this.userService.checkUserRecommendationPreferences(),
                        ]),
                    ),
                )
                .pipe(this.untilThis)
                .subscribe((results) => {
                    [this.recommendedSongs, this.userFilledPreferences] = results;
                    this.spinnerOverlayService.hide();
                    this.playerService.emitPlayerStateChange(undefined, []);

                    this.setObservableLastItem();
                });
        }
    }

    setObservableLastItem() {
        const clear = setInterval(() => {
            const target = document.querySelector(`#target${this.page * this.pageSize - 1}`);

            if (target) {
                clearInterval(clear);
                this.scrollService.setObserver().observe(target);
            }
        }, 100);
    }
}
