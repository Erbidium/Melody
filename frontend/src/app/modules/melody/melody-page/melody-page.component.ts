import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IFavouritePlaylistWithPerformers } from '@core/models/IFavouritePlaylistWithPerformers';
import { PlaylistService } from '@core/services/playlist.service';
import {forkJoin} from "rxjs";

@Component({
    selector: 'app-melody-page',
    templateUrl: './melody-page.component.html',
    styleUrls: ['./melody-page.component.sass'],
})
export class MelodyPageComponent extends BaseComponent implements OnInit {
    userPlaylists: IFavouritePlaylistWithPerformers[] = [];

    favouritePlaylists: IFavouritePlaylistWithPerformers[] = [];

    constructor(
        private playlistService: PlaylistService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit() {
        const userPlaylists = this.playlistService.getPlaylistsCreatedByUser();
        const favouritePlaylists = this.playlistService.getFavouritePlaylists();

        forkJoin([userPlaylists, favouritePlaylists])
            .pipe(this.untilThis)
            .subscribe((results) => {
                [this.userPlaylists, this.favouritePlaylists] = results;
            });
    }

    createPlaylist() {
        this.router.navigateByUrl('playlist');
    }
}
