import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AuthPageComponent } from './auth-page/auth-page.component';
import { AuthRoutingModule } from "@modules/auth/auth-routing.module";

@NgModule({
    declarations: [
        AuthPageComponent,
    ],
    imports: [
        CommonModule,
        AuthRoutingModule,
    ],
})
export class AuthModule { }
