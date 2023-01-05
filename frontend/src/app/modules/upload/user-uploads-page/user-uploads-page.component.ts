import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { columnsToDisplayWithRemoveColumn } from '@core/helpers/columns-to-display-helper';
import { ISong } from '@core/models/ISong';
import { InfiniteScrollingService } from '@core/services/infinite-scrolling.service';
import { NotificationService } from '@core/services/notification.service';
import { PlayerService } from '@core/services/player.service';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-user-uploads-page',
    templateUrl: './user-uploads-page.component.html',
    styleUrls: ['./user-uploads-page.component.sass'],
})
export class UserUploadsPageComponent extends BaseComponent implements OnInit {
    uploadedSongs: ISong[] = [];

    columnsToDisplay = columnsToDisplayWithRemoveColumn;

    currentSongIdForMusicPlayer?: number;

    page = 1;

    pageSize = 10;

    constructor(
        private songService: SongService,
        private playerService: PlayerService,
        private spinnerOverlayService: SpinnerOverlayService,
        private scrollService: InfiniteScrollingService,
        private notificationService: NotificationService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadSongsUploadedByUser(this.page, this.pageSize);
        this.playerService.playerStateEmitted$.pipe(this.untilThis).subscribe((state) => {
            this.currentSongIdForMusicPlayer = state.id;
        });

        this.scrollService
            .getObservable()
            .pipe(this.untilThis)
            .subscribe((status: { isIntersecting: boolean; id: string }) => {
                if (status.isIntersecting && status.id === `target${this.page * this.pageSize - 1}`) {
                    this.loadSongsUploadedByUser(++this.page, this.pageSize);
                }
            });
    }

    loadSongsUploadedByUser(page: number, pageSize: number, updateAllData = false) {
        this.spinnerOverlayService.show();
        this.songService
            .getSongsUploadedByUser(page, pageSize)
            .pipe(this.untilThis)
            .subscribe({
                next: (response) => {
                    this.spinnerOverlayService.hide();

                    this.uploadedSongs = updateAllData ? response : this.uploadedSongs.concat(response);

                    const clear = setInterval(() => {
                        const target = document.querySelector(`#target${page * pageSize - 1}`);

                        if (target) {
                            clearInterval(clear);
                            this.scrollService.setObserver().observe(target);
                        }
                    }, 100);
                },
                error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
            });
    }

    selectSong(songId: number) {
        this.playerService.emitPlayerStateChange(songId, this.uploadedSongs);
    }

    deleteSong(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.songService
            .deleteSong(id)
            .pipe(
                switchMap(async () => {
                    const page = Math.ceil((this.uploadedSongs.length - 1) / this.pageSize);

                    this.loadSongsUploadedByUser(1, page * this.pageSize, true);
                }),
            )
            .subscribe(() => {
                this.currentSongIdForMusicPlayer = undefined;
                this.page = Math.ceil((this.uploadedSongs.length - 1) / this.pageSize);
                this.playerService.emitPlayerStateChange(undefined, []);
            });
    }
}
