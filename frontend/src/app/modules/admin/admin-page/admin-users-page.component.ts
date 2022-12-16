import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';
import { IUserForAdmin } from '@core/models/IUserForAdmin';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { UserService } from '@core/services/user.service';

@Component({
    selector: 'app-admin-users-page',
    templateUrl: './admin-users-page.component.html',
    styleUrls: ['./admin-users-page.component.sass'],
})
export class AdminUsersPageComponent extends BaseComponent implements OnInit {
    navigationLinks = headerNavLinksAdministrator;

    users: IUserForAdmin[] = [];

    columnsToDisplay = [
        'position',
        'username',
        'email',
        'phoneNumber',
        'ban',
        'remove',
    ];

    constructor(
        private userService: UserService,
        private spinnerOverlayService: SpinnerOverlayService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit() {
        this.loadUsers();
    }

    loadUsers() {
        this.spinnerOverlayService.show();
        this.userService
            .getUsersWithoutAdminRole()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.spinnerOverlayService.hide();
                this.users = resp;
            });
    }

    navigateToUserProfilePage(id: number) {
        this.router.navigateByUrl(
            `/profile/${id}`,
        );
    }
}
