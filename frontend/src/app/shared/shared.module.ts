import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSliderModule } from '@angular/material/slider';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLinkActive, RouterLinkWithHref } from '@angular/router';
import { MaterialModule } from '@shared/material/material.module';

import { BackButtonComponent } from './components/back-button/back-button.component';
import { HeaderComponent } from './components/header/header.component';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { PlayerComponent } from './components/player/player.component';
import { SongsTableComponent } from './components/songs-table/songs-table.component';
import { HorizontalPlaylistsScrollComponent } from './components/vertical-playlists-scroll/horizontal-playlists-scroll.component';
import { HorizontalScrollDirective } from './directives/horizontal-scroll.directive';
import { DateAgoPipe } from './pipes/date-ago.pipe';
import { TextListPipe } from './pipes/text-list.pipe';
import { TimePipe } from './pipes/time.pipe';

@NgModule({
    declarations: [
        LoadingSpinnerComponent,
        HeaderComponent,
        BackButtonComponent,
        DateAgoPipe,
        TimePipe,
        PlayerComponent,
        TextListPipe,
        HorizontalPlaylistsScrollComponent,
        HorizontalScrollDirective,
        SongsTableComponent,
    ],
    exports: [
        LoadingSpinnerComponent,
        HeaderComponent,
        BackButtonComponent,
        DateAgoPipe,
        TimePipe,
        PlayerComponent,
        TextListPipe,
        HorizontalPlaylistsScrollComponent,
        SongsTableComponent,
    ],
    imports: [
        CommonModule,
        MaterialModule,
        MatToolbarModule,
        MatTabsModule,
        RouterLinkWithHref,
        RouterLinkActive,
        MatIconModule,
        MatListModule,
        MatSliderModule,
        MatTableModule,
    ],
})
export class SharedModule { }
