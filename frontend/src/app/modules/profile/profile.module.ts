import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ProfileRoutingModule } from '@modules/profile/profile-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { ProfilePageComponent } from './profile-page/profile-page.component';

@NgModule({
    declarations: [
        ProfilePageComponent,
    ],
    imports: [
        CommonModule,
        ProfileRoutingModule,
        SharedModule,
        MaterialModule,
    ],
})
export class ProfileModule { }
