import { Injectable } from '@angular/core';
import { HttpInternalService } from '@core/services/http-internal-service';
import {NotificationService} from "@core/services/notification.service";
import {tap} from "rxjs";

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService, private notificationService: NotificationService) {}

    public signUp(email: string, password: string, name: string, phone: string) {
        return this.httpService.postRequest<null>('/api/user/register', { UserName: name, email, password, PhoneNumber: phone }).pipe(
            tap({
                next: () => this.notificationService.showSuccessMessage('Authentication successful'),
                error: () =>
                    this.notificationService.showErrorMessage("You've entered wrong password! Please try again or reset your password."),
            }),
        );
    }

    public signIn(email: string, password: string) {
        return this.httpService.postRequest<null>('api/login', { email, password });
    }
}
