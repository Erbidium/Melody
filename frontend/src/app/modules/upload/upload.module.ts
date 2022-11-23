import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { UploadRoutingModule } from '@modules/upload/upload-routing-module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { UploadPageComponent } from './upload-page/upload-page.component';

@NgModule({
    declarations: [
        UploadPageComponent,
    ],
    imports: [
        CommonModule,
        UploadRoutingModule,
        SharedModule,
        MaterialModule,
        ReactiveFormsModule,
    ],
})
export class UploadModule { }
