import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MelodyPageComponent } from '@modules/melody/melody-page/melody-page.component';
import { MelodyRoutingModule } from '@modules/melody/melody-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { FavouriteSongsPageComponent } from './favourite-songs-page/favourite-songs-page.component';

@NgModule({
    declarations: [
        MelodyPageComponent,
        FavouriteSongsPageComponent,
    ],
    imports: [
        CommonModule,
        MelodyRoutingModule,
        SharedModule,
        MaterialModule,
    ],
})
export class MelodyModule { }
