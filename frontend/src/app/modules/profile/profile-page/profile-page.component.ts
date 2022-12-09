import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IFavouritePlaylistWithPerformers } from '@core/models/IFavouritePlaylistWithPerformers';
import { IUser } from '@core/models/IUser';
import { AuthService } from '@core/services/auth.service';
import { NotificationService } from '@core/services/notification.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SpinnerService } from '@core/services/spinner.service';
import { UserService } from '@core/services/user.service';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-profile-page',
    templateUrl: './profile-page.component.html',
    styleUrls: ['./profile-page.component.sass'],
})
export class ProfilePageComponent extends BaseComponent implements OnInit {
    currentUser?: IUser;

    userPlaylists: IFavouritePlaylistWithPerformers[] = [];

    constructor(
        private authService: AuthService,
        private userService: UserService,
        private spinnerService: SpinnerService,
        private playlistService: PlaylistService,
        private notificationService: NotificationService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit(): void {
        this.spinnerService.show();
        const user = this.userService.getCurrentUser();
        const playlists = this.playlistService.getPlaylistsCreatedByUser();

        forkJoin([user, playlists])
            .pipe(this.untilThis)
            .subscribe((results) => {
                [this.currentUser, this.userPlaylists] = results;
                this.spinnerService.hide();
            });
    }

    logOut() {
        this.authService
            .signOut()
            .pipe(this.untilThis)
            .subscribe(() => {
                this.notificationService.showSuccessMessage('Ти успішно вийшов з свого акаунту');
                localStorage.removeItem('access-token');
                this.router.navigateByUrl('auth');
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
