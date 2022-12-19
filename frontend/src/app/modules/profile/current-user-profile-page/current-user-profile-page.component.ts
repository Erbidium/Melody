import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IFavouritePlaylistWithPerformers } from '@core/models/IFavouritePlaylistWithPerformers';
import { IUser } from '@core/models/IUser';
import { AuthService } from '@core/services/auth.service';
import { NotificationService } from '@core/services/notification.service';
import { PlayerService } from '@core/services/player.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { UserService } from '@core/services/user.service';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-current-user-profile-page',
    templateUrl: './current-user-profile-page.component.html',
    styleUrls: ['./current-user-profile-page.component.sass'],
})
export class CurrentUserProfilePageComponent extends BaseComponent implements OnInit {
    currentUser?: IUser;

    userPlaylists: IFavouritePlaylistWithPerformers[] = [];

    constructor(
        private authService: AuthService,
        private userService: UserService,
        private spinnerOverlayService: SpinnerOverlayService,
        private playlistService: PlaylistService,
        private playerService: PlayerService,
        private notificationService: NotificationService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit(): void {
        this.spinnerOverlayService.show();
        const user = this.userService.getCurrentUser();
        const playlists = this.playlistService.getPlaylistsCreatedByUser();

        forkJoin([user, playlists])
            .pipe(this.untilThis)
            .subscribe((results) => {
                [this.currentUser, this.userPlaylists] = results;
                this.spinnerOverlayService.hide();
            });
    }

    logOut() {
        this.authService
            .signOut()
            .pipe(this.untilThis)
            .subscribe(() => {
                this.notificationService.showSuccessMessage('Ти успішно вийшов з свого акаунту');
                localStorage.removeItem('access-token');
                this.playerService.emitPlayerStateChange(undefined, []);
                sessionStorage.clear();
                this.router.navigateByUrl('auth');
            });
    }

    changePlaylistStatus(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.spinnerOverlayService.show();
        const playlist = this.userPlaylists.find((p) => p.id === id);

        if (playlist) {
            this.playlistService
                .setPlaylistStatus(id, !playlist.isFavourite)
                .pipe(switchMap(() => this.playlistService.getPlaylistsCreatedByUser()))
                .pipe(this.untilThis)
                .subscribe((userPlaylists) => {
                    this.userPlaylists = userPlaylists;
                    this.spinnerOverlayService.hide();
                });
        }
    }

    deleteAccount() {
        this.userService
            .deleteAccount()
            .pipe(this.untilThis)
            .subscribe(() => {
                this.notificationService.showSuccessMessage('Ти успішно видалив свій акаунт');
                localStorage.removeItem('access-token');
                sessionStorage.clear();
                this.router.navigateByUrl('auth');
            });
    }
}
