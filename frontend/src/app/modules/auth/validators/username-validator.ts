import { AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { UserService } from '@core/services/user.service';
import { map } from 'rxjs';

export class UsernameValidator {
    static signUpUsernameValidator(userService: UserService): AsyncValidatorFn {
        return (control: AbstractControl) =>
            userService
                .checkUsername(control.value)
                .pipe(map((result: boolean) => (result ? { userAlreadyExists: true } : null)));
    }
}
