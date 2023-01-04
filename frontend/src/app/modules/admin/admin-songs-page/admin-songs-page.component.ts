import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';
import { ISong } from '@core/models/ISong';
import { InfiniteScrollingService } from '@core/services/infinite-scrolling.service';
import { PlayerService } from '@core/services/player.service';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-admin-songs-page',
    templateUrl: './admin-songs-page.component.html',
    styleUrls: ['./admin-songs-page.component.sass'],
})
export class AdminSongsPageComponent extends BaseComponent implements OnInit {
    navigationLinks = headerNavLinksAdministrator;

    songs: ISong[] = [];

    columnsToDisplay = [
        'position',
        'name',
        'author',
        'genre',
        'date',
        'profile',
        'duration',
        'remove',
    ];

    currentSongIdForMusicPlayer?: number;

    page = 1;

    pageSize = 10;

    searchText = '';

    constructor(
        private songService: SongService,
        private playerService: PlayerService,
        private router: Router,
        private spinnerOverlayService: SpinnerOverlayService,
        private scrollService: InfiniteScrollingService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadSongs(this.page, this.pageSize);
        this.playerService
            .playerStateEmitted$
            .pipe(this.untilThis)
            .subscribe(state => {
                this.currentSongIdForMusicPlayer = state.id;
            });

        this.scrollService
            .getObservable()
            .pipe(this.untilThis)
            .subscribe((status: { isIntersecting: boolean, id: string }) => {
                if (status.isIntersecting && status.id === `target${this.page * this.pageSize - 1}`) {
                    this.loadSongs(++this.page, this.pageSize, false, this.searchText);
                }
            });
    }

    loadSongs(page: number, pageSize: number, updateAllData = false, searchText: string = '') {
        this.spinnerOverlayService.show();
        this.songService
            .getAllSongs(page, pageSize, searchText)
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.spinnerOverlayService.hide();

                this.songs = updateAllData ? resp : this.songs.concat(resp);

                const clear = setInterval(() => {
                    const target = document.querySelector(`#target${page * pageSize - 1}`);

                    if (target) {
                        clearInterval(clear);
                        this.scrollService.setObserver().observe(target);
                    }
                }, 100);
            });
    }

    selectSong(songId: number) {
        this.playerService.emitPlayerStateChange(songId, this.songs);
    }

    deleteSong(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.songService
            .deleteSongByAdministrator(id)
            .pipe(switchMap(async () => {
                const page = Math.ceil((this.songs.length - 1) / this.pageSize);

                this.loadSongs(1, page * this.pageSize, true);
            }))
            .subscribe(() => {
                this.page = Math.ceil((this.songs.length - 1) / this.pageSize);
                this.currentSongIdForMusicPlayer = undefined;
                this.playerService.emitPlayerStateChange(undefined, []);
            });
    }

    navigateToUserProfilePage(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.router.navigateByUrl(
            `/profile/${id}`,
        );
    }

    search() {
        this.loadSongs(1, this.page * this.pageSize, true, this.searchText);
    }
}
