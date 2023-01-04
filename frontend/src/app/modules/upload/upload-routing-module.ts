import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/guards/auth.guard';
import { UploadPageComponent } from '@modules/upload/upload-page/upload-page.component';
import { UserUploadsPageComponent } from '@modules/upload/user-uploads-page/user-uploads-page.component';

const routes: Routes = [
    {
        path: '',
        component: UploadPageComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'all',
        component: UserUploadsPageComponent,
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
export class UploadRoutingModule {}
