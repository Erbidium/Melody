import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import {
    RecommendationsPageComponent,
} from '@modules/recommendations/recommendations-page/recommendations-page.component';
import { RecommendationsRoutingModule } from '@modules/recommendations/recommendations-routing.module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { RecommendationsPreferencesPageComponent } from './recommendations-preferences-page/recommendations-preferences-page.component';

@NgModule({
    declarations: [
        RecommendationsPageComponent,
        RecommendationsPreferencesPageComponent,
    ],
    imports: [
        CommonModule,
        RecommendationsRoutingModule,
        SharedModule,
        MatIconModule,
        MaterialModule,
        ReactiveFormsModule,
    ],
})
export class RecommendationsModule { }
