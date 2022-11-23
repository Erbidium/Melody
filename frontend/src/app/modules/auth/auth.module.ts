import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
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
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatTabsModule,
        ReactiveFormsModule,
    ],
})
export class AuthModule { }
