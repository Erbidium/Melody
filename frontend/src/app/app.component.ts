import { Component } from '@angular/core';
import { SpinnerService } from '@core/services/spinner.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.sass'],
})
export class AppComponent {
    // eslint-disable-next-line no-empty-function
    constructor(private spinner: SpinnerService) {}
}
