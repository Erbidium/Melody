import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/guards/auth.guard';
import { StatisticsPageComponent } from '@modules/statistics/statistics-page/statistics-page.component';

const routes: Routes = [
    {
        path: '',
        component: StatisticsPageComponent,
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
export class StatisticsRoutingModule {}
