import { Injectable } from '@angular/core';
import { IUser } from '@core/models/IUser';
import { HttpInternalService } from '@core/services/http-internal-service';

@Injectable({ providedIn: 'root' })
export class UserService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public getCurrentUser() {
        return this.httpService.getRequest<IUser>('/api/user');
    }
}
