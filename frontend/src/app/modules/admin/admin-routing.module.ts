import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from '@core/guards/role.guard';
import { AdminPageComponent } from '@modules/admin/admin-page/admin-page.component';

const routes: Routes = [
    {
        path: '',
        component: AdminPageComponent,
        canActivate: [RoleGuard],
        data: {
            expectedRole: 'Admin',
        },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AdminRoutingModule {}
