import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, UrlTree } from '@angular/router';
import { AuthService } from '@core/services/auth.service';
import decode from 'jwt-decode';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class RoleGuard implements CanActivate {
    // eslint-disable-next-line no-empty-function
    constructor(private authService: AuthService, private router: Router) {}

    canActivate(
        route: ActivatedRouteSnapshot,
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        // eslint-disable-next-line prefer-destructuring
        const expectedRole = route.data['expectedRole'];
        const token = localStorage.getItem('access-token');

        if (!token) {
            return this.router.parseUrl('/melody');
        }
        const tokenPayload: {
            UserId: string;
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string[];
        } = decode(token);
        const roles = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        return this.authService.isLoggedIn() && roles.indexOf(expectedRole) !== -1
            ? true
            : this.router.parseUrl('/melody');
    }
}
