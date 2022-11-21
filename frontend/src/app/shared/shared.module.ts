import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '@shared/material/material.module';

import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { HeaderComponent } from './components/header/header.component';
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatTabsModule } from "@angular/material/tabs";
import { RouterLinkActive, RouterLinkWithHref } from "@angular/router";
import { MatIconModule } from "@angular/material/icon";

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
