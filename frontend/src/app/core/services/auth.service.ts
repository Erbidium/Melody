import { Injectable } from '@angular/core';
import { HttpInternalService } from '@core/services/http-internal-service';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public signUp(email: string, password: string, name: string, phone: string) {
        return this.httpService.postRequest<null>('/api/user/register', { UserName: name, email, password, PhoneNumber: phone });
    }

    public signIn(email: string, password: string) {
        return this.httpService.postRequest<null>('api/login', { email, password });
    }
}
