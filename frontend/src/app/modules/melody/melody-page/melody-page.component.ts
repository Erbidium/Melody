import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';

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
