import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { PlaylistRoutingModule } from '@modules/playlist/playlist-routing.module';
import { CreatePlaylistPageComponent } from './create-playlist-page/create-playlist-page.component';
import {MaterialModule} from "@shared/material/material.module";
import {ReactiveFormsModule} from "@angular/forms";
import {SharedModule} from "@shared/shared.module";

@NgModule({
    declarations: [
    CreatePlaylistPageComponent
  ],
    imports: [
        CommonModule,
        PlaylistRoutingModule,
        MaterialModule,
        ReactiveFormsModule,
        SharedModule,
    ],
})
export class PlaylistModule { }
