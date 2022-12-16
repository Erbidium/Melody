import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from '@core/guards/role.guard';
import { AdminUsersPageComponent } from '@modules/admin/admin-page/admin-users-page.component';
import { AdminSongsPageComponent } from '@modules/admin/admin-songs-page/admin-songs-page.component';

const routes: Routes = [
    {
        path: 'users',
        component: AdminUsersPageComponent,
        canActivate: [RoleGuard],
        data: {
            expectedRole: 'Admin',
        },
    },
    {
        path: 'songs',
        component: AdminSongsPageComponent,
        canActivate: [RoleGuard],
        data: {
            expectedRole: 'Admin',
        },
    },
    {
        path: '',
        redirectTo: '/melody',
        pathMatch: 'full',
    },
    {
        path: '**',
        redirectTo: '/melody',
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AdminRoutingModule {}
