import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { columnsToDisplayWithFavouriteColumn } from '@core/helpers/columns-to-display-helper';
import { ISong } from '@core/models/ISong';
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

    constructor(
        private songService: SongService,
        private spinnerOverlayService: SpinnerOverlayService,
        private playerService: PlayerService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadFavouriteUserSongs();
        this.playerService
            .playerStateEmitted$
            .pipe(this.untilThis)
            .subscribe(state => {
                this.currentSongIdForMusicPlayer = state.id;
            });
    }

    loadFavouriteUserSongs() {
        this.spinnerOverlayService.show();
        this.songService
            .getFavouriteUserSongs()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.spinnerOverlayService.hide();
                this.favouriteSongs = resp;
            });
    }

    selectSong(songId: number) {
        this.playerService.emitPlayerStateChange(songId, this.favouriteSongs);
    }

    removeSongFromFavourites(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.songService
            .removeSongFromUserFavourites(id)
            .pipe(switchMap(async () => this.loadFavouriteUserSongs()))
            .subscribe(() => {
                this.currentSongIdForMusicPlayer = undefined;
            });
    }
}
