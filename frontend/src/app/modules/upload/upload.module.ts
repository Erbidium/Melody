import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { UploadRoutingModule } from '@modules/upload/upload-routing-module';

import { UploadPageComponent } from './upload-page/upload-page.component';
import { SharedModule } from "@shared/shared.module";

@NgModule({
    declarations: [
        UploadPageComponent,
    ],
  imports: [
    CommonModule,
    UploadRoutingModule,
    SharedModule,
  ],
})
export class UploadModule { }
