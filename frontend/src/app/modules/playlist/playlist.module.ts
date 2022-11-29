import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { PlaylistRoutingModule } from '@modules/playlist/playlist-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { CreatePlaylistPageComponent } from './create-playlist-page/create-playlist-page.component';
import { PlaylistPageComponent } from './playlist-page/playlist-page.component';
import {MatTableModule} from "@angular/material/table";

@NgModule({
    declarations: [
        CreatePlaylistPageComponent,
        PlaylistPageComponent,
    ],
    imports: [
        CommonModule,
        PlaylistRoutingModule,
        MaterialModule,
        ReactiveFormsModule,
        SharedModule,
        MatTableModule,
    ],
})
export class PlaylistModule { }
