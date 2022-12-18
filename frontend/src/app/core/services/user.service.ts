import { Injectable } from '@angular/core';
import { IUser } from '@core/models/IUser';
import { IUserForAdmin } from '@core/models/IUserForAdmin';
import { HttpInternalService } from '@core/services/http-internal-service';
import { NotificationService } from '@core/services/notification.service';
import { Observable, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService, private notificationService: NotificationService) {}

    public getCurrentUser() {
        return this.httpService.getRequest<IUser>('/api/user');
    }

    public getUserById(id: number) {
        return this.httpService.getRequest<IUser>(`/api/user/${id}`);
    }

    public checkEmail(email: string): Observable<boolean> {
        const emailEncoded = encodeURIComponent(email);

        return this.httpService.getRequest<boolean>(`/api/user/check-email?email=${emailEncoded}`).pipe(
            tap({
                error: () =>
                    this.notificationService.showErrorMessage('Something went wrong. Failed to verify email exists.'),
            }),
        );
    }

    public checkUsername(username: string): Observable<boolean> {
        const usernameEncoded = encodeURIComponent(username);

        return this.httpService.getRequest<boolean>(`/api/user/check-username?username=${usernameEncoded}`).pipe(
            tap({
                error: () =>
                    this.notificationService.showErrorMessage('Something went wrong. Failed to verify username exists.'),
            }),
        );
    }

    public getUsersWithoutAdminRole() {
        return this.httpService.getRequest<IUserForAdmin[]>('/api/user/all');
    }

    public setUserBanStatus(id: number, isBanned: boolean) {
        return this.httpService.patchRequest(`/api/user/${id}/ban`, { isBanned });
    }

    public deleteUser(id: number) {
        return this.httpService.deleteRequest(`/api/user/${id}`);
    }
}
