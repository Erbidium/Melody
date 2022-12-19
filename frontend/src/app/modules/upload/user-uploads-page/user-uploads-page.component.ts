import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { columnsToDisplayWithRemoveColumn } from '@core/helpers/columns-to-display-helper';
import { ISong } from '@core/models/ISong';
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

    constructor(
        private songService: SongService,
        private playerService: PlayerService,
        private spinnerOverlayService: SpinnerOverlayService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadSongsUploadedByUser();
        this.playerService
            .playerStateEmitted$
            .pipe(this.untilThis)
            .subscribe(state => {
                this.currentSongIdForMusicPlayer = state.id;
            });
    }

    loadSongsUploadedByUser() {
        this.spinnerOverlayService.show();
        this.songService
            .getSongsUploadedByUser()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.spinnerOverlayService.hide();
                this.uploadedSongs = resp;
            });
    }

    selectSong(songId: number) {
        this.playerService.emitPlayerStateChange(songId, this.uploadedSongs);
    }

    deleteSong(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.songService
            .deleteSong(id)
            .pipe(switchMap(async () => this.loadSongsUploadedByUser()))
            .subscribe(() => {
                this.currentSongIdForMusicPlayer = undefined;
                this.playerService.emitPlayerStateChange(undefined, []);
            });
    }
}
