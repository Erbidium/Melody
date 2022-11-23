import { Injectable } from '@angular/core';
import { HttpInternalService } from '@core/services/http-internal-service';
import { NotificationService } from '@core/services/notification.service';
import {first, Observable, tap} from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService, private notificationService: NotificationService) {}

    public signUp(email: string, password: string, name: string, phone: string) {
        return this.httpService.postRequest<null>('/api/user/register', { UserName: name, email, password, PhoneNumber: phone }).pipe(
            tap({
                next: () => this.notificationService.showSuccessMessage('Registration successful'),
                error: () =>
                    this.notificationService.showErrorMessage("You've entered invalid data! Please try again."),
            }),
        );
    }

    public signIn(email: string, password: string) {
        return this.httpService.postRequest<{ accessToken: string }>('/api/login', { email, password }).pipe(
            tap({
                next: (accessToken: { accessToken: string }) => {
                    console.log(accessToken);
                    this.notificationService.showSuccessMessage('Authentication successful');
                    localStorage.setItem('access-token', accessToken.accessToken);
                },
                error: (e) => {
                    console.log(e);
                    this.notificationService.showErrorMessage("You've entered wrong password or email! Please try again.");
                },
            }),
        );
    }

    public getAccessToken() {
        return localStorage.getItem('access-token');
    }

    public isLoggedIn(): boolean {
        return !!localStorage.getItem('access-token');
    }

    public signOut(): Observable<void> {
        return this.httpService.postRequest<void>('/api/logout', {}).pipe(
            tap({
                next: () => {
                    localStorage.removeItem('access-token');
                },
                error: (e) => this.notificationService.showErrorMessage(e.message),
            }),
        );
    }

    public refreshToken() {
        return this.httpService.postRequest<{ accessToken: string }>('/api/login/refresh', {}).pipe(
            tap({
                next: (token) => {
                    localStorage.setItem('access-token', token.accessToken);
                },
                error: (e) => this.notificationService.showErrorMessage(e.message),
            }),
        );
    }
}
