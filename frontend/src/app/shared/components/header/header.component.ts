import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.sass'],
})
export class HeaderComponent extends BaseComponent {
    public navLinks = [
        { path: '/melody', label: 'Головна' },
        { path: '/recommendations', label: 'Рекомендації' },
        { path: '/statistics', label: 'Статистика' },
        { path: '/upload', label: 'Завантажити пісню' },
    ];

    constructor(
        private router: Router,
    ) {
        super();
    }

    public navigateToProfile() {
        this.router.navigateByUrl(
            '/profile',
        );
    }
}
