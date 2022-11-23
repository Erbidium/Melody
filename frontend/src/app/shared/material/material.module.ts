import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        MatCardModule,
        MatSnackBarModule,
    ],
    exports: [
        MatCardModule,
        MatSnackBarModule,
    ],
})
export class MaterialModule { }
