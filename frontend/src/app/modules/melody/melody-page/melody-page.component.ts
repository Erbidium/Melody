import { Component } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import {Router} from "@angular/router";

@Component({
    selector: 'app-melody-page',
    templateUrl: './melody-page.component.html',
    styleUrls: ['./melody-page.component.sass'],
})
export class MelodyPageComponent extends BaseComponent {
    constructor(private router: Router) {
        super();
    }

    createPlaylist() {
        this.router.navigateByUrl('playlist');
    }
}
