import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from '@core/base/base.component';
import { IGenre } from '@core/models/IGenre';
import { NotificationService } from '@core/services/notification.service';
import { SongService } from '@core/services/song.service';

@Component({
    selector: 'app-upload-page',
    templateUrl: './upload-page.component.html',
    styleUrls: ['./upload-page.component.sass'],
})
export class UploadPageComponent extends BaseComponent implements OnInit {
    genres: IGenre[] = [];

    selectedGenre?: IGenre;

    fileToUpload?: File;

    private songDuration: number = 0;

    constructor(
        private songService: SongService,
        private notificationService: NotificationService,
        private router: Router,
    ) {
        super();
    }

    ngOnInit() {
        this.songService
            .getAllGenres()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.genres = resp;
            });
    }

    public uploadForm = new FormGroup({
        name: new FormControl('', {
            validators: [Validators.required, Validators.minLength(2), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
        author: new FormControl('', {
            validators: [Validators.required, Validators.minLength(2), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
        year: new FormControl('', {
            validators: [Validators.required],
            updateOn: 'blur',
        }),
    });

    async loadSoundFile(event: Event) {
        const target = event.target as HTMLInputElement;

        // eslint-disable-next-line prefer-destructuring
        const fileList = target.files as FileList;

        if (fileList.length > 0) {
            if (fileList[0].type !== 'audio/mpeg') {
                this.notificationService.showErrorMessage('Завантажено файл з некоректним розширенням');

                return;
            }
            // eslint-disable-next-line prefer-destructuring
            this.fileToUpload = fileList[0];
            const context = new AudioContext();
            const arrayBuffer = await fileList[0].arrayBuffer();
            const buffer = await context.decodeAudioData(<ArrayBuffer> arrayBuffer);

            this.songDuration = Math.floor(buffer.duration);
        }
    }

    upload() {
        if (this.fileToUpload && this.selectedGenre && this.songDuration > 0) {
            const formData = new FormData();

            formData.append('Name', this.uploadForm.value.name!);
            formData.append('AuthorName', this.uploadForm.value.author!);
            formData.append('Year', this.uploadForm.value.year!);
            formData.append('GenreId', this.selectedGenre.id.toString());
            formData.append('DurationInSeconds', this.songDuration.toString());
            formData.append('uploadedSoundFile', this.fileToUpload);
            this.songService.createSong(formData)
                .pipe(this.untilThis)
                .subscribe({
                    next: () => {
                        this.notificationService.showSuccessMessage('Пісню було успішно завантажено');
                        this.uploadForm.reset();
                        this.selectedGenre = undefined;
                        this.fileToUpload = undefined;
                    },
                    error: (e) => this.notificationService.showErrorMessage(e),
                });
        }
    }

    reset() {
        this.router.navigateByUrl('melody');
    }
}
