import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IPlaylistWithPerformers } from '@core/models/IPlaylistWithPerformers';
import { IUser } from '@core/models/IUser';
import { AuthService } from '@core/services/auth.service';
import { NotificationService } from '@core/services/notification.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SpinnerService } from '@core/services/spinner.service';
import { UserService } from '@core/services/user.service';

@Component({
    selector: 'app-profile-page',
    templateUrl: './profile-page.component.html',
    styleUrls: ['./profile-page.component.sass'],
})
export class ProfilePageComponent extends BaseComponent implements OnInit {
    currentUser?: IUser;

    userPlaylists: IPlaylistWithPerformers[] = [];

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
        this.userService
            .getCurrentUser()
            .pipe(this.untilThis)
            .subscribe((user) => {
                this.currentUser = user;
                this.spinnerService.hide();
            });
        this.playlistService
            .getPlaylistsCreatedByUser()
            .pipe(this.untilThis)
            .subscribe((playlists) => {
                this.userPlaylists = playlists;
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
}
