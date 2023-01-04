import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'melody',
        pathMatch: 'full',
    },
    {
        path: 'auth',
        loadChildren: () => import('./modules/auth/auth.module').then((m) => m.AuthModule),
    },
    {
        path: 'melody',
        loadChildren: () => import('./modules/melody/melody.module').then((m) => m.MelodyModule),
    },
    {
        path: 'upload',
        loadChildren: () => import('./modules/upload/upload.module').then((m) => m.UploadModule),
    },
    {
        path: 'profile',
        loadChildren: () => import('./modules/profile/profile.module').then((m) => m.ProfileModule),
    },
    {
        path: 'recommendations',
        loadChildren: () =>
            import('./modules/recommendations/recommendations.module').then((m) => m.RecommendationsModule),
    },
    {
        path: 'statistics',
        loadChildren: () => import('./modules/statistics/statistics.module').then((m) => m.StatisticsModule),
    },
    {
        path: 'playlist',
        loadChildren: () => import('./modules/playlist/playlist.module').then((m) => m.PlaylistModule),
    },
    {
        path: 'admin',
        loadChildren: () => import('./modules/admin/admin.module').then((m) => m.AdminModule),
    },
    { path: '**', redirectTo: '/melody', pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}
