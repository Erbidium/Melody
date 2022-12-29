import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { AdminRoutingModule } from '@modules/admin/admin-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { AdminUsersPageComponent } from './admin-page/admin-users-page.component';
import { AdminSongsPageComponent } from './admin-songs-page/admin-songs-page.component';
import {FormsModule} from "@angular/forms";

@NgModule({
    declarations: [
        AdminUsersPageComponent,
        AdminSongsPageComponent,
    ],
    imports: [
        CommonModule,
        AdminRoutingModule,
        SharedModule,
        MatTableModule,
        MaterialModule,
        MatSlideToggleModule,
        FormsModule,
    ],
})
export class AdminModule { }
