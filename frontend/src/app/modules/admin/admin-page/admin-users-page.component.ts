import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';
import { IUserForAdmin } from '@core/models/IUserForAdmin';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { UserService } from '@core/services/user.service';
import { switchMap } from 'rxjs/operators';
import {PlayerService} from "@core/services/player.service";

@Component({
    selector: 'app-admin-users-page',
    templateUrl: './admin-users-page.component.html',
    styleUrls: ['./admin-users-page.component.sass'],
})
export class AdminUsersPageComponent extends BaseComponent implements OnInit {
    navigationLinks = headerNavLinksAdministrator;

    users: IUserForAdmin[] = [];

    columnsToDisplay = ['position', 'username', 'email', 'phoneNumber', 'ban', 'remove'];

    constructor(
        private userService: UserService,
        private spinnerService: SpinnerOverlayService,
        private playerService: PlayerService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit() {
        this.loadUsers();
    }

    loadUsers() {
        this.spinnerService.show();
        this.userService
            .getUsersWithoutAdminRole()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.spinnerService.hide();
                this.users = resp;
            });
    }

    navigateToUserProfilePage(id: number) {
        this.router.navigateByUrl(`/profile/${id}`);
    }

    changeUserBan(id: number) {
        const user = this.users.find((u) => u.id === id);

        if (user) {
            this.spinnerService.show();
            this.userService
                .setUserBanStatus(id, !user.isBanned)
                .pipe(switchMap(() => this.userService.getUsersWithoutAdminRole()))
                .pipe(this.untilThis)
                .subscribe((results) => {
                    this.users = results;
                    this.spinnerService.hide();
                    if (!user.isBanned) {
                        this.playerService.emitPlayerStateChange(undefined, []);
                    }
                });
        }
    }

    deleteUser(id: number, event: MouseEvent) {
        event.stopPropagation();
        const user = this.users.find((u) => u.id === id);

        if (user) {
            this.spinnerService.show();
            this.userService
                .deleteUser(id)
                .pipe(switchMap(() => this.userService.getUsersWithoutAdminRole()))
                .pipe(this.untilThis)
                .subscribe((results) => {
                    this.users = results;
                    this.spinnerService.hide();
                    this.playerService.emitPlayerStateChange(undefined, []);
                });
        }
    }
}
