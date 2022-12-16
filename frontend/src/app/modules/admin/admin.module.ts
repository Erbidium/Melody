import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AdminRoutingModule } from '@modules/admin/admin-routing.module';
import { SharedModule } from '@shared/shared.module';

import { AdminUsersPageComponent } from './admin-page/admin-users-page.component';

@NgModule({
    declarations: [
        AdminUsersPageComponent,
    ],
    imports: [
        CommonModule,
        AdminRoutingModule,
        SharedModule,
    ],
})
export class AdminModule { }
