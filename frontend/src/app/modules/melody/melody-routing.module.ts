import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/guards/auth.guard';
import { MelodyPageComponent } from '@modules/melody/melody-page/melody-page.component';

const routes: Routes = [
    {
        path: '',
        component: MelodyPageComponent,
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
export class MelodyRoutingModule {}
