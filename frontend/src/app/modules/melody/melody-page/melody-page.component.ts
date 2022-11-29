import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IPlaylistWithPerformers } from '@core/models/IPlaylistWithPerformers';
import { PlaylistService } from '@core/services/playlist.service';

@Component({
    selector: 'app-melody-page',
    templateUrl: './melody-page.component.html',
    styleUrls: ['./melody-page.component.sass'],
})
export class MelodyPageComponent extends BaseComponent implements OnInit {
    userPlaylists: IPlaylistWithPerformers[] = [];

    constructor(
        private playlistService: PlaylistService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit(): void {
        this.playlistService
            .getPlaylistsCreatedByUser()
            .pipe(this.untilThis)
            .subscribe((playlists) => {
                this.userPlaylists = playlists;
            });
    }

    createPlaylist() {
        this.router.navigateByUrl('playlist');
    }
}
