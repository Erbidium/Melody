import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IPlaylistWithPerformers } from '@core/models/IPlaylistWithPerformers';

@Component({
    selector: 'app-vertical-playlists-scroll',
    templateUrl: './vertical-playlists-scroll.component.html',
    styleUrls: ['./vertical-playlists-scroll.component.sass'],
})
export class VerticalPlaylistsScrollComponent extends BaseComponent {
    @Input() userPlaylists: IPlaylistWithPerformers[] = [];

    constructor(private router: Router) {
        super();
    }

    navigateToPlaylist(id: number) {
        this.router.navigateByUrl(`playlist/${id}`);
    }
}
