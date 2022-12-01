import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IPlaylistWithPerformers } from '@core/models/IPlaylistWithPerformers';

@Component({
    selector: 'app-horizontal-playlists-scroll',
    templateUrl: './horizontal-playlists-scroll.component.html',
    styleUrls: ['./horizontal-playlists-scroll.component.sass'],
})
export class HorizontalPlaylistsScrollComponent extends BaseComponent {
    @Input() userPlaylists: IPlaylistWithPerformers[] = [];

    constructor(private router: Router) {
        super();
    }

    navigateToPlaylist(id: number) {
        this.router.navigateByUrl(`playlist/${id}`);
    }
}
