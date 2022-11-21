import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {BaseComponent} from "@core/base/base.component";
import {AuthService} from "@core/services/auth.service";

@Component({
    selector: 'app-auth-page',
    templateUrl: './auth-page.component.html',
    styleUrls: ['./auth-page.component.sass'],
})
export class AuthPageComponent extends BaseComponent {

    public signUpForm = new FormGroup({
        username: new FormControl('', {
            validators: [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
        email: new FormControl('', {
            validators: [Validators.required, Validators.email],
            updateOn: 'blur',
        }),
        password: new FormControl('', {
            validators: [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
        phone: new FormControl('', {
            validators: [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
    });

    constructor(private authService: AuthService) {
        super();
    }

    private setCredentialsIncorrect(): void {
        this.signUpForm.get('email')?.setErrors({ incorrectCredentials: true });
        this.signUpForm.get('name')?.setErrors({ incorrectCredentials: true });
        this.signUpForm.get('password')?.setErrors({ incorrectCredentials: true });
    }

    public onSignUp(): void {
        if (this.signUpForm.valid) {
            const email = this.signUpForm.value.email!;
            const password = this.signUpForm.value.password!;
            const name = this.signUpForm.value.username!;
            const phone = this.signUpForm.value.phone!;

            this.authService
                .signUp(email, password, name, phone)
                .subscribe({ error: () => this.setCredentialsIncorrect() });
        }
    }
}
