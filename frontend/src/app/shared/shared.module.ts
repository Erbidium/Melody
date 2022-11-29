import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSliderModule } from '@angular/material/slider';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLinkActive, RouterLinkWithHref } from '@angular/router';
import { MaterialModule } from '@shared/material/material.module';

import { BackButtonComponent } from './components/back-button/back-button.component';
import { HeaderComponent } from './components/header/header.component';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { PlayerComponent } from './components/player/player.component';
import { DateAgoPipe } from './pipes/date-ago.pipe';
import { TimePipe } from './pipes/time.pipe';
import { TextListPipe } from './pipes/text-list.pipe';

@NgModule({
    declarations: [
        LoadingSpinnerComponent,
        HeaderComponent,
        BackButtonComponent,
        DateAgoPipe,
        TimePipe,
        PlayerComponent,
        TextListPipe,
    ],
    exports: [
        LoadingSpinnerComponent,
        HeaderComponent,
        BackButtonComponent,
        DateAgoPipe,
        TimePipe,
        PlayerComponent,
        TextListPipe,
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
    ],
})
export class SharedModule { }
