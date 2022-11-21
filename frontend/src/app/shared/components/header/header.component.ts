import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.sass'],
})
export class HeaderComponent {
    public navLinks = [
        { path: '/melody', label: 'Main' },
        { path: '/recommendations', label: 'Recommendations' },
        { path: '/statistics', label: 'Statistics' },
        { path: '/upload', label: 'Load song' },
    ];
}
