import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthRoutingModule } from '@modules/auth/auth-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { AuthPageComponent } from './auth-page/auth-page.component';

@NgModule({
    declarations: [
        AuthPageComponent,
    ],
    imports: [
        CommonModule,
        AuthRoutingModule,
        SharedModule,
        MaterialModule,
        ReactiveFormsModule,
    ],
})
export class AuthModule { }
