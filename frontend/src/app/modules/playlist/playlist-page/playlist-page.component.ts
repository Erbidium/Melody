import { Clipboard } from '@angular/cdk/clipboard';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IPlaylist } from '@core/models/IPlaylist';
import { NotificationService } from '@core/services/notification.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SpinnerService } from '@core/services/spinner.service';

@Component({
    selector: 'app-playlist-page',
    templateUrl: './playlist-page.component.html',
    styleUrls: ['./playlist-page.component.sass'],
})
export class PlaylistPageComponent extends BaseComponent {
    public playlist?: IPlaylist;

    private id: number | undefined;

    columnsToDisplay = ['position', 'name', 'author', 'genre', 'heart', 'date', 'duration'];

    currentSongIdForMusicPlayer?: number;

    private currentUrl?: string;

    constructor(
        private activateRoute: ActivatedRoute,
        private playlistService: PlaylistService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService,
        private clipboard: Clipboard,
    ) {
        super();
        this.activateRoute.params.subscribe((params) => {
            this.id = params['id'];
            this.spinnerService.show();
            if (this.id) {
                this.playlistService
                    .getPlaylistById(this.id)
                    .pipe(this.untilThis)
                    .subscribe((playlistResponse) => {
                        this.playlist = playlistResponse;
                        this.spinnerService.hide();
                    });
            }
        });
    }

    selectSong(songId: number) {
        this.currentSongIdForMusicPlayer = songId;
    }

    copyPlaylistLink() {
        if (this.playlist?.id) {
            this.clipboard.copy(window.location.href);
            this.notificationService.showSuccessMessage('Playlist link is successfully copied to clipboard!');
        } else {
            this.notificationService.showErrorMessage('Cannot generate link for the playlist');
        }
    }
}