import { AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { UserService } from '@core/services/user.service';
import { map } from 'rxjs';

export class EmailValidator {
    static loginEmailValidator(userService: UserService): AsyncValidatorFn {
        return (control: AbstractControl) =>
            userService
                .checkEmail(control.value)
                .pipe(map((result: boolean) => (result ? null : { userInNotExist: true })));
    }

    static signUpEmailValidator(userService: UserService): AsyncValidatorFn {
        return (control: AbstractControl) =>
            userService
                .checkEmail(control.value)
                .pipe(map((result: boolean) => (result ? { userAlreadyExists: true } : null)));
    }
}
