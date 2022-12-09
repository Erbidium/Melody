import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IFavouritePlaylistWithPerformers } from '@core/models/IFavouritePlaylistWithPerformers';

@Component({
    selector: 'app-horizontal-playlists-scroll',
    templateUrl: './horizontal-playlists-scroll.component.html',
    styleUrls: ['./horizontal-playlists-scroll.component.sass'],
})
export class HorizontalPlaylistsScrollComponent extends BaseComponent {
    @Input() userPlaylists: IFavouritePlaylistWithPerformers[] = [];

    @Input() heartButtonForLikes: boolean = false;

    constructor(private router: Router) {
        super();
    }

    navigateToPlaylist(id: number) {
        this.router.navigateByUrl(`playlist/${id}`);
    }

    getMatIconText(playlist: IFavouritePlaylistWithPerformers) {
        if (!this.heartButtonForLikes) {
            return 'favorite';
        }

        return playlist.isFavourite ? 'favorite' : 'favorite_border';
    }
}
