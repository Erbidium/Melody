import { Clipboard } from '@angular/cdk/clipboard';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IPlaylist } from '@core/models/IPlaylist';
import { ISong } from '@core/models/ISong';
import { IUser } from '@core/models/IUser';
import { NotificationService } from '@core/services/notification.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SongService } from '@core/services/song.service';
import { SpinnerService } from '@core/services/spinner.service';
import { UserService } from '@core/services/user.service';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-playlist-page',
    templateUrl: './playlist-page.component.html',
    styleUrls: ['./playlist-page.component.sass'],
})
export class PlaylistPageComponent extends BaseComponent implements OnInit {
    public playlist?: IPlaylist;

    currentUser?: IUser;

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
        private userService: UserService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService,
        private clipboard: Clipboard,
        private router: Router,
    ) {
        super();
    }

    ngOnInit() {
        this.activateRoute.params.pipe(this.untilThis).subscribe((params) => {
            this.id = params['id'];
            if (this.id) {
                this.spinnerService.show();
                const playlist = this.playlistService.getPlaylistById(this.id);
                const songs = this.playlistService.getSongsToAddToPlaylist(this.id);
                const user = this.userService.getCurrentUser();

                forkJoin([playlist, songs, user])
                    .pipe(this.untilThis)
                    .subscribe((results) => {
                        [this.playlist, this.newSongsToAdd, this.currentUser] = results;
                        this.spinnerService.hide();
                    });
            }
        });
    }

    selectSong = (songId: number) => {
        this.currentSongIdForMusicPlayer = songId;
    };

    copyPlaylistLink() {
        if (this.id) {
            this.clipboard.copy(window.location.href);
            this.notificationService.showSuccessMessage('Playlist link is successfully copied to clipboard!');
        } else {
            this.notificationService.showErrorMessage('Cannot generate link for the playlist');
        }
    }

    deleteSongFromPlaylist = (id: number, event: MouseEvent) => {
        if (this.playlist) {
            event.stopPropagation();
            this.spinnerService.show();
            this.playlistService
                .removeSongFromPlaylist(id, this.playlist.id)
                .pipe(
                    switchMap(() => {
                        this.currentSongIdForMusicPlayer = undefined;

                        return forkJoin([
                            this.playlistService.getPlaylistById(this.playlist!.id),
                            this.playlistService.getSongsToAddToPlaylist(this.playlist!.id),
                        ]);
                    }),
                )
                .pipe(this.untilThis)
                .subscribe((results) => {
                    [this.playlist, this.newSongsToAdd] = results;
                    this.spinnerService.hide();
                });
        }
    };

    changeSongStatus = (id: number, event: MouseEvent) => {
        if (this.playlist && this.currentUser) {
            event.stopPropagation();
            this.spinnerService.show();
            const song = this.playlist.songs.find(s => s.id === id);

            if (song) {
                this.songService
                    .setSongStatus(id, !song.isFavourite)
                    .pipe(
                        switchMap(() => forkJoin([
                            this.playlistService.getPlaylistById(this.playlist!.id),
                            this.playlistService.getSongsToAddToPlaylist(this.playlist!.id),
                        ])),
                    )
                    .pipe(this.untilThis)
                    .subscribe((results) => {
                        [this.playlist, this.newSongsToAdd] = results;
                        this.spinnerService.hide();
                    });
            }
        }
    };

    deletePlaylist() {
        if (this.playlist) {
            this.playlistService
                .deletePlaylist(this.playlist.id)
                .pipe(this.untilThis)
                .subscribe(() => {
                    this.router.navigateByUrl('melody');
                });
        }
    }

    addSongs() {
        if (this.playlist) {
            this.spinnerService.show();
            this.playlistService
                .addSongs(this.playlist.id, this.addSongsPlaylistForm.value.songs as unknown as number[])
                .pipe(switchMap(() => forkJoin([
                    this.playlistService.getPlaylistById(this.playlist!.id),
                    this.playlistService.getSongsToAddToPlaylist(this.playlist!.id),
                ])))
                .pipe(this.untilThis)
                .subscribe({
                    next: (results) => {
                        [this.playlist, this.newSongsToAdd] = results;
                        this.spinnerService.hide();
                        this.addSongsPlaylistForm.reset();
                        this.notificationService.showSuccessMessage('Нові пісні успішно додано!');
                    },
                    error: () => this.notificationService.showErrorMessage('Сталася помилка під час спроби додати нові пісні'),
                });
        }
    }
}
