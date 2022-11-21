import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { BaseComponent } from './base/base.component';
import {HttpClientModule} from "@angular/common/http";

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        HttpClientModule,
    ],
    declarations: [
        BaseComponent,
    ],
})
export class CoreModule { }
