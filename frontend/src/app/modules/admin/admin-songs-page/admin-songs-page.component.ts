import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';
import { ISong } from '@core/models/ISong';
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

    constructor(
        private songService: SongService,
        private playerService: PlayerService,
        private router: Router,
        private spinnerOverlayService: SpinnerOverlayService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadSongs();
        this.playerService
            .playerStateEmitted$
            .pipe(this.untilThis)
            .subscribe(state => {
                this.currentSongIdForMusicPlayer = state.id;
            });
    }

    loadSongs() {
        this.spinnerOverlayService.show();
        this.songService
            .getAllSongs()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.spinnerOverlayService.hide();
                this.songs = resp;
            });
    }

    selectSong(songId: number) {
        this.playerService.emitPlayerStateChange(songId, this.songs);
    }

    deleteSong(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.songService
            .deleteSongByAdministrator(id)
            .pipe(switchMap(async () => this.loadSongs()))
            .subscribe(() => {
                this.currentSongIdForMusicPlayer = undefined;
            });
    }

    navigateToUserProfilePage(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.router.navigateByUrl(
            `/profile/${id}`,
        );
    }
}
