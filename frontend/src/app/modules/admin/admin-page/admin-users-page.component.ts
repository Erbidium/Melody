import { Component } from '@angular/core';
import { headerNavLinksAdministrator } from '@core/helpers/header-helpers';

@Component({
    selector: 'app-admin-users-page',
    templateUrl: './admin-users-page.component.html',
    styleUrls: ['./admin-users-page.component.sass'],
})
export class AdminUsersPageComponent {
    navigationLinks = headerNavLinksAdministrator;
}
