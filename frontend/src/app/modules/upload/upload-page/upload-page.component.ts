import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { IGenre } from '@core/models/IGenre';
import { SongService } from '@core/services/song.service';
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
    selector: 'app-upload-page',
    templateUrl: './upload-page.component.html',
    styleUrls: ['./upload-page.component.sass'],
})
export class UploadPageComponent extends BaseComponent implements OnInit {
    genres: IGenre[] = [];

    selectedGenre?: IGenre;

    constructor(private songService: SongService) {
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
}
