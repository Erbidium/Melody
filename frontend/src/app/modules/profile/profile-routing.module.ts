import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/guards/auth.guard';
import { CurrentUserProfilePageComponent } from '@modules/profile/current-user-profile-page/current-user-profile-page.component';
import { ProfilePageComponent } from '@modules/profile/profile-page/profile-page.component';

const routes: Routes = [
    {
        path: '',
        component: CurrentUserProfilePageComponent,
        canActivate: [AuthGuard],
    },
    {
        path: ':id',
        component: ProfilePageComponent,
        canActivate: [AuthGuard],
    },
    {
        path: '**',
        redirectTo: '',
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProfileRoutingModule {}
