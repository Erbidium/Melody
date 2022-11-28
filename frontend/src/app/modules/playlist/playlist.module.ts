import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { PlaylistRoutingModule } from '@modules/playlist/playlist-routing.module';
import { CreatePlaylistPageComponent } from './create-playlist-page/create-playlist-page.component';

@NgModule({
    declarations: [
    CreatePlaylistPageComponent
  ],
    imports: [
        CommonModule,
        PlaylistRoutingModule,
    ],
})
export class PlaylistModule { }
