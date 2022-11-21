import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UploadPageComponent } from '@modules/upload/upload-page/upload-page.component';

const routes: Routes = [
    {
        path: '',
        component: UploadPageComponent,
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
