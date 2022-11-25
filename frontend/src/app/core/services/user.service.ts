import { Injectable } from '@angular/core';
import { IUser } from '@core/models/IUser';
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

    public checkEmail(email: string): Observable<boolean> {
        const emailEncoded = encodeURIComponent(email);

        return this.httpService.getRequest<boolean>(`/api/user/check-email?email=${emailEncoded}`).pipe(
            tap({
                error: () =>
                    this.notificationService.showErrorMessage('Something went wrong. Failed to verify email exists.'),
            }),
        );
    }
}
