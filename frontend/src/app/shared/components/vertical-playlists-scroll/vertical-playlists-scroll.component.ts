import { Component, Input } from '@angular/core';
import { IPlaylistWithPerformers } from '@core/models/IPlaylistWithPerformers';

@Component({
    selector: 'app-vertical-playlists-scroll',
    templateUrl: './vertical-playlists-scroll.component.html',
    styleUrls: ['./vertical-playlists-scroll.component.sass'],
})
export class VerticalPlaylistsScrollComponent {
    @Input() userPlaylists: IPlaylistWithPerformers[] = [];
}
