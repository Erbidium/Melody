import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IFavouritePlaylistWithPerformers } from '@core/models/IFavouritePlaylistWithPerformers';
import { IUser } from '@core/models/IUser';
import { NotificationService } from '@core/services/notification.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { UserService } from '@core/services/user.service';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-profile-page',
    templateUrl: './profile-page.component.html',
    styleUrls: ['./profile-page.component.sass'],
})
export class ProfilePageComponent extends BaseComponent implements OnInit {
    private id: number | undefined;

    user?: IUser;

    userPlaylists: IFavouritePlaylistWithPerformers[] = [];

    constructor(
        private userService: UserService,
        private playlistService: PlaylistService,
        private activateRoute: ActivatedRoute,
        private spinnerService: SpinnerOverlayService,
        private notificationService: NotificationService,
    ) {
        super();
    }

    ngOnInit() {
        this.activateRoute.params.pipe(this.untilThis).subscribe((params) => {
            this.id = params['id'];
            if (this.id) {
                this.spinnerService.show();
                const user = this.userService.getUserById(this.id);
                const playlists = this.playlistService.getPlaylistsCreatedByUser();

                forkJoin([user, playlists])
                    .pipe(this.untilThis)
                    .subscribe({
                        next: (results) => {
                            [this.user, this.userPlaylists] = results;
                            this.spinnerService.hide();
                        },
                        error: () => {
                            this.spinnerService.hide();
                            this.notificationService.showErrorMessage('Не вдалося завантажити профіль користувача');
                        } });
            }
        });
    }

    changePlaylistStatus(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.spinnerService.show();
        const playlist = this.userPlaylists.find((p) => p.id === id);

        if (playlist) {
            this.playlistService
                .setPlaylistStatus(id, !playlist.isFavourite)
                .pipe(switchMap(() => this.playlistService.getPlaylistsCreatedByUser()))
                .pipe(this.untilThis)
                .subscribe((userPlaylists) => {
                    this.userPlaylists = userPlaylists;
                    this.spinnerService.hide();
                });
        }
    }
}
