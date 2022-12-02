import { Clipboard } from '@angular/cdk/clipboard';
import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Route, Router} from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IPlaylist } from '@core/models/IPlaylist';
import { NotificationService } from '@core/services/notification.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SpinnerService } from '@core/services/spinner.service';
import {switchMap} from "rxjs/operators";
import {ISong} from "@core/models/ISong";
import {SongService} from "@core/services/song.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
    selector: 'app-playlist-page',
    templateUrl: './playlist-page.component.html',
    styleUrls: ['./playlist-page.component.sass'],
})
export class PlaylistPageComponent extends BaseComponent implements OnInit {
    public playlist?: IPlaylist;

    newSongsToAdd: ISong[] = [];

    private id: number | undefined;

    columnsToDisplay = ['position', 'name', 'author', 'genre', 'heart', 'date', 'duration'];

    currentSongIdForMusicPlayer?: number;

    public addSongsPlaylistForm = new FormGroup({
        songs: new FormControl([] as number[], {
            validators: [Validators.required],
            updateOn: 'blur',
        }),
    });

    constructor(
        private activateRoute: ActivatedRoute,
        private playlistService: PlaylistService,
        private songService: SongService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService,
        private clipboard: Clipboard,
        private router: Router,
    ) {
        super();
        this.activateRoute.params.subscribe((params) => {
            this.id = params['id'];
            this.loadPlaylist();
        });
    }

    ngOnInit(): void {
        this.loadSongsForPlaylist();
    }

    loadPlaylist() {
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
    }

    loadSongsForPlaylist() {
        if (this.id) {
            this.playlistService
                .getSongsToAddToPlaylist(this.id)
                .pipe(this.untilThis)
                .subscribe((resp) => {
                    this.newSongsToAdd = resp;
                });
        }
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

    deleteSongFromPlaylist(id: number, event: MouseEvent) {
        if (this.playlist) {
            event.stopPropagation();
            this.playlistService
                .removeSongFromPlaylist(id, this.playlist.id)
                .pipe(switchMap(async () => this.loadPlaylist()))
                .pipe(switchMap(async () => this.loadSongsForPlaylist()))
                .subscribe(() => {
                    this.currentSongIdForMusicPlayer = undefined;
                });
        }
    }

    deletePlaylist() {
        if (this.playlist) {
            this.playlistService
                .deletePlaylist(this.playlist.id)
                .subscribe(() => {
                    this.router.navigateByUrl('melody');
                });
        }
    }
}
