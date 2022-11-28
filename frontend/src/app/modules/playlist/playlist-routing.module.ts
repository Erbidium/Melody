import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/guards/auth.guard';
import { CreatePlaylistPageComponent } from '@modules/playlist/create-playlist-page/create-playlist-page.component';

const routes: Routes = [
    {
        path: 'new',
        component: CreatePlaylistPageComponent,
        canActivate: [AuthGuard],
    },
    {
        path: '**',
        redirectTo: 'new',
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PlaylistRoutingModule {}
