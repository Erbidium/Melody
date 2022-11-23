import { Component } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import {AuthService} from "@core/services/auth.service";
import {Router} from "@angular/router";

@Component({
    selector: 'app-profile-page',
    templateUrl: './profile-page.component.html',
    styleUrls: ['./profile-page.component.sass'],
})
export class ProfilePageComponent extends BaseComponent {
    constructor(private authService: AuthService, private router: Router) {
        super();
    }

    logOut() {
        this.authService.signOut();
        this.router.navigateByUrl('auth');
    }
}
