import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';
import { IUserForAdmin } from '@core/models/IUserForAdmin';
import { InfiniteScrollingService } from '@core/services/infinite-scrolling.service';
import { NotificationService } from '@core/services/notification.service';
import { PlayerService } from '@core/services/player.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { UserService } from '@core/services/user.service';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-admin-users-page',
    templateUrl: './admin-users-page.component.html',
    styleUrls: ['./admin-users-page.component.sass'],
})
export class AdminUsersPageComponent extends BaseComponent implements OnInit {
    navigationLinks = headerNavLinksAdministrator;

    users: IUserForAdmin[] = [];

    columnsToDisplay = ['position', 'username', 'email', 'phoneNumber', 'ban', 'remove'];

    page = 1;

    pageSize = 10;

    searchText = '';

    constructor(
        private userService: UserService,
        private spinnerService: SpinnerOverlayService,
        private playerService: PlayerService,
        private router: Router,
        private scrollService: InfiniteScrollingService,
        private notificationService: NotificationService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadUsers(this.page, this.pageSize);

        this.scrollService
            .getObservable()
            .pipe(this.untilThis)
            .subscribe((status: { isIntersecting: boolean; id: string }) => {
                if (status.isIntersecting && status.id === `target${this.page * this.pageSize - 1}`) {
                    this.loadUsers(++this.page, this.pageSize, false, this.searchText);
                }
            });
    }

    loadUsers(page: number, pageSize: number, updateAllData = false, searchText: string = '') {
        this.spinnerService.show();
        this.userService
            .getUsersWithoutAdminRole(page, pageSize, searchText)
            .pipe(this.untilThis)
            .subscribe({
                next: (resp) => {
                    this.spinnerService.hide();

                    this.users = updateAllData ? resp : this.users.concat(resp);

                    const clear = setInterval(() => {
                        const target = document.querySelector(`#target${page * pageSize - 1}`);

                        if (target) {
                            clearInterval(clear);
                            this.scrollService.setObserver().observe(target);
                        }
                    }, 100);
                },
                error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
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
                .pipe(
                    switchMap(() => this.userService.getUsersWithoutAdminRole(1, this.page * this.pageSize)),
                )
                .pipe(this.untilThis)
                .subscribe({
                    next: (results) => {
                        this.users = results;
                        this.spinnerService.hide();
                        if (!user.isBanned) {
                            this.playerService.emitPlayerStateChange(undefined, []);
                        }
                    },
                    error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
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
                .pipe(
                    switchMap(() => {
                        const page = Math.max(1, Math.ceil((this.users.length - 1) / this.pageSize));

                        return this.userService.getUsersWithoutAdminRole(1, page * this.pageSize);
                    }),
                )
                .pipe(this.untilThis)
                .subscribe({
                    next: (results) => {
                        this.users = results;
                        this.spinnerService.hide();
                        this.page = Math.max(1, Math.ceil(this.users.length / this.pageSize));
                        this.playerService.emitPlayerStateChange(undefined, []);
                    },
                    error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
                });
        }
    }

    search() {
        this.loadUsers(1, this.page * this.pageSize, true, this.searchText);
    }
}
