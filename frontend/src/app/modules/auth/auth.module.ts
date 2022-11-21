import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AuthPageComponent } from './auth-page/auth-page.component';
import { AuthRoutingModule } from "@modules/auth/auth-routing.module";
import { SharedModule } from "@shared/shared.module";
import { MaterialModule } from "@shared/material/material.module";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatTabsModule } from "@angular/material/tabs";

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
    MatTabsModule
  ],
})
export class AuthModule { }
