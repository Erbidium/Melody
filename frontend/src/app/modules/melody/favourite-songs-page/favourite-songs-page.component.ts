import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { columnsToDisplayWithFavouriteColumn } from '@core/helpers/columns-to-display-helper';
import { ISong } from '@core/models/ISong';
import { InfiniteScrollingService } from '@core/services/infinite-scrolling.service';
import { PlayerService } from '@core/services/player.service';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-favourite-songs-page',
    templateUrl: './favourite-songs-page.component.html',
    styleUrls: ['./favourite-songs-page.component.sass'],
})
export class FavouriteSongsPageComponent extends BaseComponent implements OnInit {
    favouriteSongs: ISong[] = [];

    columnsToDisplay = columnsToDisplayWithFavouriteColumn;

    currentSongIdForMusicPlayer?: number;

    page = 1;

    pageSize = 10;

    constructor(
        private songService: SongService,
        private spinnerOverlayService: SpinnerOverlayService,
        private playerService: PlayerService,
        private scrollService: InfiniteScrollingService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadFavouriteUserSongs(this.page, this.pageSize);
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
                    this.loadFavouriteUserSongs(++this.page, this.pageSize);
                }
            });
    }

    loadFavouriteUserSongs(page: number, pageSize: number, updateAllData = false) {
        this.spinnerOverlayService.show();
        this.songService
            .getFavouriteUserSongs(page, pageSize)
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.spinnerOverlayService.hide();

                this.favouriteSongs = resp;
                this.favouriteSongs = updateAllData ? resp : this.favouriteSongs.concat(resp);

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
        this.playerService.emitPlayerStateChange(songId, this.favouriteSongs);
    }

    removeSongFromFavourites(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.songService
            .removeSongFromUserFavourites(id)
            .pipe(switchMap(async () => {
                this.page = Math.ceil((this.favouriteSongs.length - 1) / this.pageSize);
                this.loadFavouriteUserSongs(1, this.page * this.pageSize, true);
            }))
            .subscribe(() => {
                this.currentSongIdForMusicPlayer = undefined;
                this.playerService.emitPlayerStateChange(undefined, []);
            });
    }
}
