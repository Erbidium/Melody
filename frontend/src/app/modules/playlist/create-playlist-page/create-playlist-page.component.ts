import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { ISong } from '@core/models/ISong';
import { SongService } from '@core/services/song.service';
import {FormControl, FormGroup, Validators} from "@angular/forms";

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
        songs: new FormControl('', {
            validators: [Validators.required],
            updateOn: 'blur',
        }),
    });

    constructor(private songService: SongService) {
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

    create() {
        console.log(this.createPlaylistForm.value.songs);
    }
}
