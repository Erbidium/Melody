import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';
import { ISong } from '@core/models/ISong';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { switchMap } from 'rxjs/operators';
import {Router} from "@angular/router";

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
        private router: Router,
        private spinnerOverlayService: SpinnerOverlayService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadSongs();
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
        this.currentSongIdForMusicPlayer = songId;
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
