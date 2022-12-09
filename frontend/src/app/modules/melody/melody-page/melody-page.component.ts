import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IFavouritePlaylistWithPerformers } from '@core/models/IFavouritePlaylistWithPerformers';
import { PlaylistService } from '@core/services/playlist.service';
import { SpinnerService } from '@core/services/spinner.service';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

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
        private spinnerService: SpinnerService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit() {
        forkJoin([
            this.playlistService.getPlaylistsCreatedByUser(),
            this.playlistService.getFavouritePlaylists(),
        ])
            .pipe(this.untilThis)
            .subscribe((results) => {
                [this.userPlaylists, this.favouritePlaylists] = results;
            });
    }

    createPlaylist() {
        this.router.navigateByUrl('playlist');
    }

    changePlaylistStatus(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.spinnerService.show();
        const playlist = this.userPlaylists.find(p => p.id === id);

        if (playlist) {
            this.playlistService.setPlaylistStatus(id, !playlist.isFavourite)
                .pipe(
                    switchMap(() => forkJoin([
                        this.playlistService.getPlaylistsCreatedByUser(),
                        this.playlistService.getFavouritePlaylists(),
                    ])),
                )
                .pipe(this.untilThis)
                .subscribe((results) => {
                    [this.userPlaylists, this.favouritePlaylists] = results;
                    this.spinnerService.hide();
                });
        }
    }

    deletePlaylistFromFavourites(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.spinnerService.show();
        this.playlistService.removePlaylistFromUserFavourites(id)
            .pipe(
                switchMap(() => forkJoin([
                    this.playlistService.getPlaylistsCreatedByUser(),
                    this.playlistService.getFavouritePlaylists(),
                ])),
            )
            .pipe(this.untilThis)
            .subscribe((results) => {
                [this.userPlaylists, this.favouritePlaylists] = results;
                this.spinnerService.hide();
            });
    }
}
