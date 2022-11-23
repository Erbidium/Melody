import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {
    RecommendationsPageComponent,
} from '@modules/recommendations/recommendations-page/recommendations-page.component';
import { RecommendationsRoutingModule } from '@modules/recommendations/recommendations-routing.module';
import {SharedModule} from "@shared/shared.module";

@NgModule({
    declarations: [
        RecommendationsPageComponent,
    ],
    imports: [
        CommonModule,
        RecommendationsRoutingModule,
        SharedModule,
    ],
})
export class RecommendationsModule { }
