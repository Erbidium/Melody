import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { UploadRoutingModule } from '@modules/upload/upload-routing-module';

import { UploadPageComponent } from './upload-page/upload-page.component';

@NgModule({
    declarations: [
        UploadPageComponent,
    ],
    imports: [
        CommonModule,
        UploadRoutingModule,
    ],
})
export class UploadModule { }
