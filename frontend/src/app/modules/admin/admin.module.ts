import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AdminRoutingModule } from '@modules/admin/admin-routing.module';

import { AdminPageComponent } from './admin-page/admin-page.component';

@NgModule({
    declarations: [
        AdminPageComponent,
    ],
    imports: [
        CommonModule,
        AdminRoutingModule,
    ],
})
export class AdminModule { }
