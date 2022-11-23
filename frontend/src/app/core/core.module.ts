import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import { BaseComponent } from './base/base.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        HttpClientModule,
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    ],
    declarations: [
        BaseComponent,
    ],
})
export class CoreModule { }
