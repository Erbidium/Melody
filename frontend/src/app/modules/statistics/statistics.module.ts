import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { StatisticsRoutingModule } from '@modules/statistics/statistics-routing.module';

import { StatisticsPageComponent } from './statistics-page/statistics-page.component';

@NgModule({
    declarations: [
        StatisticsPageComponent,
    ],
    imports: [
        CommonModule,
        StatisticsRoutingModule,
    ],
})
export class StatisticsModule { }
