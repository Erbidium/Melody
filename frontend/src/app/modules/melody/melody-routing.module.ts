import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@core/guards/auth.guard';
import { MelodyPageComponent } from '@modules/melody/melody-page/melody-page.component';
import {FavouriteSongsPageComponent} from "@modules/melody/favourite-songs-page/favourite-songs-page.component";

const routes: Routes = [
    {
        path: '',
        component: MelodyPageComponent,
        canActivate: [AuthGuard],
    },
    {
        path: 'favourite',
        component: FavouriteSongsPageComponent,
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
