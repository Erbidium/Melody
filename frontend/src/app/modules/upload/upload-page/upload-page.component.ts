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

    constructor(private songService: SongService, private notificationService: NotificationService, private router: Router) {
        super();
    }

    ngOnInit(): void {
        this.songService
            .getAll()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.genres = resp;
            });
    }

    public uploadForm = new FormGroup({
        name: new FormControl('', {
            validators: [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
        author: new FormControl('', {
            validators: [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
        year: new FormControl('', {
            validators: [Validators.required, Validators.minLength(8), Validators.maxLength(30)],
            updateOn: 'blur',
        }),
    });

    loadSoundFile(event: Event) {
        const target = event.target as HTMLInputElement;

        // eslint-disable-next-line prefer-destructuring
        const fileList = target.files as FileList;

        if (fileList.length > 0) {
            // eslint-disable-next-line prefer-destructuring
            this.fileToUpload = fileList[0];
        }
    }

    upload() {
        if (this.fileToUpload && this.selectedGenre) {
            const formData = new FormData();

            formData.append('Name', this.uploadForm.value.name!);
            formData.append('AuthorName', this.uploadForm.value.author!);
            formData.append('Year', this.uploadForm.value.year!);
            formData.append('GenreId', this.selectedGenre.id.toString());
            formData.append('uploadedSoundFile', this.fileToUpload);
            this.songService.createSong(formData)
                .subscribe({
                    next: () => {
                        this.notificationService.showSuccessMessage('Song was successfully uploaded');
                        this.uploadForm.reset();
                        this.selectedGenre = undefined;
                    },
                    error: (e) => this.notificationService.showErrorMessage(e),
                });
        }
    }

    reset() {
        this.router.navigateByUrl('melody');
    }
}
