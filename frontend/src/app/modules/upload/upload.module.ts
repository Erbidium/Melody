import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { UploadRoutingModule } from '@modules/upload/upload-routing-module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { UploadPageComponent } from './upload-page/upload-page.component';
import { UserUploadsPageComponent } from './user-uploads-page/user-uploads-page.component';

@NgModule({
    declarations: [
        UploadPageComponent,
        UserUploadsPageComponent,
    ],
    imports: [
        CommonModule,
        UploadRoutingModule,
        SharedModule,
        MaterialModule,
        ReactiveFormsModule,
        MatTableModule,
    ],
})
export class UploadModule { }
