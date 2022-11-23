import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { UploadRoutingModule } from '@modules/upload/upload-routing-module';
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
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        MatCardModule,
        MatSelectModule,
        MatIconModule,
        MatButtonModule,
    ],
})
export class UploadModule { }
