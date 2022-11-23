import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ProfileRoutingModule } from '@modules/profile/profile-routing.module';

import { ProfilePageComponent } from './profile-page/profile-page.component';
import {SharedModule} from "@shared/shared.module";

@NgModule({
    declarations: [
        ProfilePageComponent,
    ],
    imports: [
        CommonModule,
        ProfileRoutingModule,
        SharedModule,
    ],
})
export class ProfileModule { }
