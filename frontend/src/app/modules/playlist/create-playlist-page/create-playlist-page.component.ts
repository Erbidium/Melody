import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BaseComponent } from '@core/base/base.component';
import { ISong } from '@core/models/ISong';
import { NotificationService } from '@core/services/notification.service';
import { PlaylistService } from '@core/services/playlist.service';
import { SongService } from '@core/services/song.service';

@Component({
    selector: 'app-create-playlist-page',
    templateUrl: './create-playlist-page.component.html',
    styleUrls: ['./create-playlist-page.component.sass'],
})
export class CreatePlaylistPageComponent extends BaseComponent implements OnInit {
    uploadedSongs: ISong[] = [];

    public createPlaylistForm = new FormGroup({
        name: new FormControl('', {
            validators: [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
        songs: new FormControl([] as number[], {
            validators: [Validators.required],
            updateOn: 'blur',
        }),
    });

    constructor(
        private songService: SongService,
        private playlistService: PlaylistService,
        private notificationService: NotificationService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.loadSongsUploadedByUser();
    }

    loadSongsUploadedByUser() {
        this.songService
            .getSongsUploadedByUser()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.uploadedSongs = resp;
            });
    }

    initialFormValue = {
        name: '',
        songs: [] as number[],
    };

    create() {
        this.playlistService
            .createPlaylist(
                this.createPlaylistForm.value.name ?? '',
                this.createPlaylistForm.value.songs as unknown as number[],
            )
            .pipe(this.untilThis)
            .subscribe({
                next: () => {
                    this.notificationService.showSuccessMessage('Плейлист успішно створено!');
                    this.createPlaylistForm.reset();
                },
                error: () => this.notificationService.showErrorMessage('Сталася помилка під час спроби створити плейлист'),
            });
    }
}
