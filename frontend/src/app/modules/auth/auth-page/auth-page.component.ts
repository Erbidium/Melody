import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { AuthService } from '@core/services/auth.service';
import { PlayerService } from '@core/services/player.service';
import { UserService } from '@core/services/user.service';
import { EmailValidator } from '@modules/auth/validators/email-validator';
import { UsernameValidator } from '@modules/auth/validators/username-validator';

@Component({
    selector: 'app-auth-page',
    templateUrl: './auth-page.component.html',
    styleUrls: ['./auth-page.component.sass'],
})
export class AuthPageComponent extends BaseComponent {
    public signUpForm = new FormGroup(
        {
            username: new FormControl(
                '',
                [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
                [UsernameValidator.signUpUsernameValidator(this.userService)],
            ),
            email: new FormControl(
                '',
                [Validators.required, Validators.email],
                [EmailValidator.signUpEmailValidator(this.userService)],
            ),
            password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(30)]),
            phone: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(30)]),
        },
        {
            updateOn: 'blur',
        },
    );

    public signInForm = new FormGroup(
        {
            emailRegistered: new FormControl(
                '',
                [Validators.required, Validators.email],
                [EmailValidator.loginEmailValidator(this.userService)],
            ),
            passwordRegistered: new FormControl('', [
                Validators.required,
                Validators.minLength(8),
                Validators.maxLength(30),
            ]),
        },
        {
            updateOn: 'blur',
        },
    );

    constructor(
        private authService: AuthService,
        private userService: UserService,
        private playerService: PlayerService,
        private router: Router,
    ) {
        super();
    }

    private setCredentialsIncorrect(): void {
        this.signUpForm.get('email')?.setErrors({ incorrectCredentials: true });
        this.signUpForm.get('username')?.setErrors({ incorrectCredentials: true });
        this.signUpForm.get('password')?.setErrors({ incorrectCredentials: true });
        this.signUpForm.get('phone')?.setErrors({ incorrectCredentials: true });
    }

    public onSignUp(): void {
        if (this.signUpForm.valid) {
            const email = this.signUpForm.value.email!;
            const password = this.signUpForm.value.password!;
            const name = this.signUpForm.value.username!;
            const phone = this.signUpForm.value.phone!;

            this.authService.signUp(email, password, name, phone).subscribe({
                next: () => {
                    this.signUpForm.reset();
                },
                error: () => this.setCredentialsIncorrect(),
            });
        }
    }

    public onSignIn(): void {
        if (this.signInForm.valid) {
            this.authService
                .signIn(this.signInForm.value.emailRegistered!, this.signInForm.value.passwordRegistered!)
                .subscribe({
                    next: () => {
                        this.signInForm.reset();
                        sessionStorage.clear();
                        this.playerService.emitPlayerStateChange(undefined, []);
                        this.router.navigateByUrl('melody');
                    },
                    error: () => this.setCredentialsIncorrect(),
                });
        }
    }
}
