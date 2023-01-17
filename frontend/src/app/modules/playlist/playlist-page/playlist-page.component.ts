import { Clipboard } from '@angular/cdk/clipboard';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { columnsToDisplayWithFavouriteColumn } from '@core/helpers/columns-to-display-helper';
import { IFavouritePlaylist } from '@core/models/IFavouritePlaylist';
import { ISong } from '@core/models/ISong';
import { IUser } from '@core/models/IUser';
import { InfiniteScrollingService } from '@core/services/infinite-scrolling.service';
import { NotificationService } from '@core/services/notification.service';
import { PlayerService } from '@core/services/player.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { UserService } from '@core/services/user.service';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-playlist-page',
    templateUrl: './playlist-page.component.html',
    styleUrls: ['./playlist-page.component.sass'],
})
export class PlaylistPageComponent extends BaseComponent implements OnInit {
    public playlist?: IFavouritePlaylist;

    currentUser?: IUser;

    newSongsToAdd: ISong[] = [];

    private id: number | undefined;

    columnsToDisplay = columnsToDisplayWithFavouriteColumn;

    currentSongIdForMusicPlayer?: number;

    public addSongsPlaylistForm = new FormGroup({
        songs: new FormControl([] as number[], {
            validators: [Validators.required],
            updateOn: 'blur',
        }),
    });

    page = 1;

    pageSize = 10;

    constructor(
        private activateRoute: ActivatedRoute,
        private playlistService: PlaylistService,
        private songService: SongService,
        private userService: UserService,
        private spinnerService: SpinnerOverlayService,
        private notificationService: NotificationService,
        private playerService: PlayerService,
        private scrollService: InfiniteScrollingService,
        private clipboard: Clipboard,
        private router: Router,
    ) {
        super();
    }

    ngOnInit() {
        this.activateRoute.params.pipe(this.untilThis).subscribe((params) => {
            this.id = params['id'];
            if (this.id) {
                this.page = 1;
                this.pageSize = 10;
                this.spinnerService.show();
                const playlist = this.playlistService.getPlaylistById(this.id, this.page, this.pageSize);
                const songs = this.playlistService.getSongsToAddToPlaylist(this.id);
                const user = this.userService.getCurrentUser();

                forkJoin([playlist, songs, user])
                    .pipe(this.untilThis)
                    .subscribe((results) => {
                        [this.playlist, this.newSongsToAdd, this.currentUser] = results;

                        this.spinnerService.hide();

                        this.setObservableLastItem();
                    });
            }
        });
        this.playerService.playerStateEmitted$.pipe(this.untilThis).subscribe((state) => {
            this.currentSongIdForMusicPlayer = state.id;
        });
        this.scrollService
            .getObservable()
            .pipe(this.untilThis)
            .subscribe((status: { isIntersecting: boolean; id: string }) => {
                if (status.isIntersecting && status.id === `target${this.page * this.pageSize - 1}`) {
                    this.spinnerService.show();
                    this.playlistService
                        .getPlaylistById(this.playlist!.id, ++this.page, this.pageSize)
                        .pipe(this.untilThis)
                        .subscribe((playlist) => {
                            this.spinnerService.hide();
                            this.playlist!.songs = this.playlist!.songs.concat(playlist.songs);
                            this.setObservableLastItem();
                        });
                }
            });
    }

    selectSong(songId: number) {
        if (this.playlist) {
            this.playerService.emitPlayerStateChange(songId, this.playlist.songs);
        }
    }

    copyPlaylistLink() {
        if (this.id) {
            this.clipboard.copy(window.location.href);
            this.notificationService.showSuccessMessage('Посилання на плейлист успішно скопійовано у буфер обміну!');
        } else {
            this.notificationService.showErrorMessage('Не вдалося створити посилання для плейлисту');
        }
    }

    deleteSongFromPlaylist(id: number, event: MouseEvent) {
        if (this.playlist) {
            event.stopPropagation();
            this.spinnerService.show();
            this.playlistService
                .removeSongFromPlaylist(id, this.playlist.id)
                .pipe(
                    switchMap(() => {
                        this.currentSongIdForMusicPlayer = undefined;
                        const page = Math.max(1, Math.ceil((this.playlist!.songs.length - 1) / this.pageSize));

                        return forkJoin([
                            this.playlistService.getPlaylistById(this.playlist!.id, 1, page * this.pageSize),
                            this.playlistService.getSongsToAddToPlaylist(this.playlist!.id),
                        ]);
                    }),
                )
                .pipe(this.untilThis)
                .subscribe({
                    next: (results) => {
                        [this.playlist, this.newSongsToAdd] = results;
                        this.page = Math.max(1, Math.ceil((this.playlist!.songs.length) / this.pageSize));
                        this.spinnerService.hide();
                        this.playerService.emitPlayerStateChange(undefined, []);

                        this.setObservableLastItem();
                    },
                    error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
                });
        }
    }

    changeSongStatus(id: number, event: MouseEvent) {
        if (this.playlist && this.currentUser) {
            event.stopPropagation();
            this.spinnerService.show();
            const song = this.playlist.songs.find((s) => s.id === id);

            if (song) {
                this.songService
                    .setSongStatus(id, !song.isFavourite)
                    .pipe(
                        switchMap(() =>
                            forkJoin([
                                this.playlistService.getPlaylistById(this.playlist!.id, 1, this.page * this.pageSize),
                                this.playlistService.getSongsToAddToPlaylist(this.playlist!.id),
                            ])),
                    )
                    .pipe(this.untilThis)
                    .subscribe({
                        next: (results) => {
                            [this.playlist, this.newSongsToAdd] = results;
                            this.spinnerService.hide();
                            this.playerService.emitPlayerStateChange(undefined, []);

                            this.setObservableLastItem();
                        },
                        error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
                    });
            }
        }
    }

    deletePlaylist() {
        if (this.playlist) {
            this.playlistService
                .deletePlaylist(this.playlist.id)
                .pipe(this.untilThis)
                .subscribe({
                    next: () => {
                        this.playerService.emitPlayerStateChange(undefined, []);
                        this.router.navigateByUrl('melody');
                    },
                    error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
                });
        }
    }

    addSongs() {
        if (this.playlist) {
            this.spinnerService.show();
            this.playlistService
                .addSongs(this.playlist.id, this.addSongsPlaylistForm.value.songs as unknown as number[])
                .pipe(
                    switchMap(() =>
                        forkJoin([
                            this.playlistService.getPlaylistById(this.playlist!.id, 1, this.page * this.pageSize),
                            this.playlistService.getSongsToAddToPlaylist(this.playlist!.id),
                        ])),
                )
                .pipe(this.untilThis)
                .subscribe({
                    next: (results) => {
                        [this.playlist, this.newSongsToAdd] = results;
                        this.spinnerService.hide();
                        this.addSongsPlaylistForm.reset();
                        this.playerService.emitPlayerStateChange(undefined, []);
                        this.notificationService.showSuccessMessage('Нові пісні успішно додано!');
                    },
                    error: () =>
                        this.notificationService.showErrorMessage('Сталася помилка під час спроби додати нові пісні'),
                });
        }
    }

    heartButtonClickHandler($event: { id: number; event: MouseEvent }) {
        if (this.currentUser && this.playlist) {
            if (this.currentUser.id === this.playlist.authorId) {
                this.deleteSongFromPlaylist($event.id, $event.event);
            } else {
                this.changeSongStatus($event.id, $event.event);
            }
        }
    }

    changePlaylistStatus() {
        if (this.playlist && this.currentUser) {
            this.spinnerService.show();

            this.playlistService
                .setPlaylistStatus(this.playlist.id, !this.playlist.isFavourite)
                .pipe(
                    switchMap(() =>
                        forkJoin([
                            this.playlistService.getPlaylistById(this.playlist!.id, 1, this.page * this.pageSize),
                            this.playlistService.getSongsToAddToPlaylist(this.playlist!.id),
                        ])),
                )
                .pipe(this.untilThis)
                .subscribe({
                    next: (results) => {
                        [this.playlist, this.newSongsToAdd] = results;
                        this.spinnerService.hide();
                    },
                    error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
                });
        }
    }

    setObservableLastItem() {
        const clear = setInterval(() => {
            const target = document.querySelector(`#target${this.page * this.pageSize - 1}`);

            if (target) {
                clearInterval(clear);
                this.scrollService.setObserver().observe(target);
            }
        }, 100);
    }
}
