import {Component, OnInit} from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { environment } from '@env/environment';
import {DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';

@Component({
    selector: 'app-statistics-page',
    templateUrl: './statistics-page.component.html',
    styleUrls: ['./statistics-page.component.sass'],
})
export class StatisticsPageComponent extends BaseComponent implements OnInit {
    private statisticsUrl = environment.statisticsUrl;

    urlSafe: SafeResourceUrl | undefined;

    constructor(private sanitizer: DomSanitizer) {
        super();
    }

    ngOnInit() {
        this.urlSafe = this.sanitizer.bypassSecurityTrustResourceUrl(this.statisticsUrl);
    }
}
