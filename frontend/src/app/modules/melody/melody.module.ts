import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MelodyPageComponent } from '@modules/melody/melody-page/melody-page.component';
import { MelodyRoutingModule } from '@modules/melody/melody-routing.module';
import {SharedModule} from "@shared/shared.module";

@NgModule({
    declarations: [
        MelodyPageComponent,
    ],
    imports: [
        CommonModule,
        MelodyRoutingModule,
        SharedModule,
    ],
})
export class MelodyModule { }
