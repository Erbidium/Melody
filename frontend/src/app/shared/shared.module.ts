import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLinkActive, RouterLinkWithHref } from '@angular/router';
import { MaterialModule } from '@shared/material/material.module';

import { HeaderComponent } from './components/header/header.component';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';

@NgModule({
    declarations: [
        LoadingSpinnerComponent,
        HeaderComponent,
    ],
    exports: [
        LoadingSpinnerComponent,
        HeaderComponent,
    ],
    imports: [
        CommonModule,
        MaterialModule,
        MatToolbarModule,
        MatTabsModule,
        RouterLinkWithHref,
        RouterLinkActive,
        MatIconModule,
    ],
})
export class SharedModule { }
