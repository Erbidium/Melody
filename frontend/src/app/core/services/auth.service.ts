import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpInternalService } from '@core/services/http-internal-service';
import { NotificationService } from '@core/services/notification.service';
import { tap } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService, private notificationService: NotificationService) {}

    signUp(email: string, password: string, name: string, phone: string) {
        return this.httpService.postRequest<null>('/api/user/register', { UserName: name, email, password, PhoneNumber: phone }).pipe(
            tap({
                next: () => this.notificationService.showSuccessMessage('Реєстрація успішна'),
                error: () =>
                    this.notificationService.showErrorMessage('Ти ввів некоректні дані! Спробуй знову'),
            }),
        );
    }

    signIn(email: string, password: string) {
        return this.httpService.postRequest<{ accessToken: string }>('/api/login', { email, password }).pipe(
            tap({
                next: (accessToken: { accessToken: string }) => {
                    this.notificationService.showSuccessMessage('Вхід успішний');
                    localStorage.setItem('access-token', accessToken.accessToken);
                },
                error: (e) => {
                    if (e instanceof HttpErrorResponse && e.status === 403) {
                        this.notificationService.showWarningMessage(
                            'Твій акаунт заблоковано',
                        );
                    } else {
                        this.notificationService.showErrorMessage('Ти ввів неправильний пароль або пошту! Спробуй знову');
                    }
                },
            }),
        );
    }

    getAccessToken() {
        return localStorage.getItem('access-token');
    }

    isLoggedIn(): boolean {
        return !!localStorage.getItem('access-token');
    }

    signOut() {
        return this.httpService.postRequest<void>('/api/logout', {}).pipe(
            tap({
                error: (e) => this.notificationService.showErrorMessage(e.message),
            }),
        );
    }

    refreshToken() {
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
