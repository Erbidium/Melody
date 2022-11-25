import {Component, OnInit} from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import {SongService} from "@core/services/song.service";
import {ISong} from "@core/models/ISong";

@Component({
    selector: 'app-user-uploads-page',
    templateUrl: './user-uploads-page.component.html',
    styleUrls: ['./user-uploads-page.component.sass'],
})
export class UserUploadsPageComponent extends BaseComponent implements OnInit {
    uploadedSongs: ISong[] = [];

    columnsToDisplay = ['name', 'author'];

    currentSongIdForMusicPlayer?: number;

    constructor(private songService: SongService) {
        super();
    }

    ngOnInit(): void {
        this.songService
            .getSongsUploadedByUser()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.uploadedSongs = resp;
            });
    }

    selectSong(songId: number) {
        this.currentSongIdForMusicPlayer = songId;
    }
}
