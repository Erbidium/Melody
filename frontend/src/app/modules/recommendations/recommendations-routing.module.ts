import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/guards/auth.guard';
import {
    RecommendationsPageComponent,
} from '@modules/recommendations/recommendations-page/recommendations-page.component';
import {
    RecommendationsPreferencesPageComponent,
} from '@modules/recommendations/recommendations-preferences-page/recommendations-preferences-page.component';

const routes: Routes = [
    {
        path: '',
        component: RecommendationsPageComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'preferences',
        component: RecommendationsPreferencesPageComponent,
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
export class RecommendationsRoutingModule {}
