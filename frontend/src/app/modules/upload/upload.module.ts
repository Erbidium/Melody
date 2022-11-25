import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { UploadRoutingModule } from '@modules/upload/upload-routing-module';
import { MaterialModule } from '@shared/material/material.module';
import { SharedModule } from '@shared/shared.module';

import { UploadPageComponent } from './upload-page/upload-page.component';
import { UserUploadsPageComponent } from './user-uploads-page/user-uploads-page.component';
import {MatTableModule} from "@angular/material/table";

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
