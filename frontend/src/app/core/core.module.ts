import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { BaseComponent } from './base/base.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
    ],
    declarations: [
        BaseComponent,
    ],
})
export class CoreModule { }
