import { Injectable } from '@angular/core';
import { IUser } from '@core/models/IUser';
import { IUserForAdmin } from '@core/models/IUserForAdmin';
import { HttpInternalService } from '@core/services/http-internal-service';
import { NotificationService } from '@core/services/notification.service';
import { Observable, tap } from 'rxjs';
import {ISong} from "@core/models/ISong";

@Injectable({ providedIn: 'root' })
export class UserService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService, private notificationService: NotificationService) {}

    getCurrentUser() {
        return this.httpService.getRequest<IUser>('/api/user');
    }

    getUserById(id: number) {
        return this.httpService.getRequest<IUser>(`/api/user/${id}`);
    }

    checkEmail(email: string): Observable<boolean> {
        const emailEncoded = encodeURIComponent(email);

        return this.httpService.getRequest<boolean>(`/api/user/check-email?email=${emailEncoded}`).pipe(
            tap({
                error: () =>
                    this.notificationService.showErrorMessage('Something went wrong. Failed to verify email exists.'),
            }),
        );
    }

    checkUsername(username: string): Observable<boolean> {
        const usernameEncoded = encodeURIComponent(username);

        return this.httpService.getRequest<boolean>(`/api/user/check-username?username=${usernameEncoded}`).pipe(
            tap({
                error: () =>
                    this.notificationService.showErrorMessage('Something went wrong. Failed to verify username exists.'),
            }),
        );
    }

    getUsersWithoutAdminRole(page: number, pageSize: number, searchText?: string) {
        if (searchText) {
            return this.httpService.getRequest<IUserForAdmin[]>('/api/user/all', { page, pageSize, searchText });
        }

        return this.httpService.getRequest<IUserForAdmin[]>('/api/user/all', { page, pageSize });
    }

    setUserBanStatus(id: number, isBanned: boolean) {
        return this.httpService.patchRequest(`/api/user/${id}/ban`, { isBanned });
    }

    deleteUser(id: number) {
        return this.httpService.deleteRequest(`/api/user/${id}`);
    }

    deleteAccount() {
        return this.httpService.deleteRequest('/api/user');
    }
}
