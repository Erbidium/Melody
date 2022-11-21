import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AuthPageComponent } from './auth-page/auth-page.component';
import { AuthRoutingModule } from "@modules/auth/auth-routing.module";
import { SharedModule } from "@shared/shared.module";
import { MaterialModule } from "@shared/material/material.module";

@NgModule({
    declarations: [
        AuthPageComponent,
    ],
    imports: [
        CommonModule,
        AuthRoutingModule,
      SharedModule,
      MaterialModule
    ],
})
export class AuthModule { }
