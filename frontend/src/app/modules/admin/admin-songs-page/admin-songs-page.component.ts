import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';
import { ISong } from '@core/models/ISong';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';

@Component({
    selector: 'app-admin-songs-page',
    templateUrl: './admin-songs-page.component.html',
    styleUrls: ['./admin-songs-page.component.sass'],
})
export class AdminSongsPageComponent extends BaseComponent implements OnInit {
    navigationLinks = headerNavLinksAdministrator;

    songs: ISong[] = [];

    constructor(
        private songService: SongService,
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
}
