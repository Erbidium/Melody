import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { UploadRoutingModule } from '@modules/upload/upload-routing-module';

import { UploadPageComponent } from './upload-page/upload-page.component';
import { SharedModule } from "@shared/shared.module";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {ReactiveFormsModule} from "@angular/forms";
import {MatCardModule} from "@angular/material/card";
import {MatSelectModule} from "@angular/material/select";

@NgModule({
    declarations: [
        UploadPageComponent,
    ],
    imports: [
        CommonModule,
        UploadRoutingModule,
        SharedModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        MatCardModule,
        MatSelectModule,
    ],
})
export class UploadModule { }
