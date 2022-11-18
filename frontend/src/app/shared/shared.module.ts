import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '@shared/material/material.module';
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';

@NgModule({
    declarations: [
        LoadingSpinnerComponent,
    ],
    exports: [
        LoadingSpinnerComponent,
    ],
    imports: [
        CommonModule,
        MaterialModule,
    ],
})
export class SharedModule { }
