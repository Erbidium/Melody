import { Component, Input } from '@angular/core';
import { MatSliderChange } from '@angular/material/slider';
import { BaseComponent } from '@core/base/base.component';
import { StreamState } from '@core/interfaces/stream-state';
import { IBaseSong } from '@core/models/IBaseSong';
import { AudioService } from '@core/services/audio.service';
import { SongService } from '@core/services/song.service';

@Component({
    selector: 'app-player',
    templateUrl: './player.component.html',
    styleUrls: ['./player.component.sass'],
})
export class PlayerComponent extends BaseComponent {
    @Input() files: Array<IBaseSong> = [];

    state?: StreamState;

    currentSongId?: number;

    constructor(
        public audioService: AudioService,
        public songService: SongService,
    ) {
        super();
        this.audioService
            .getState()
            .pipe(this.untilThis)
            .subscribe((state) => {
                this.state = state;
            });
    }

    playStream(url: string) {
        this.audioService.playStream(url).pipe(this.untilThis).subscribe(() => {});
    }

    @Input() set currentSongIdSetter(songId: number | undefined) {
        if (songId && songId !== this.currentSongId) {
            this.openFile(songId);
        } else {
            this.stop();
            this.currentSongId = undefined;
        }
    }

    openFile(fileId: number) {
        this.currentSongId = fileId;
        this.audioService.stop();
        this.songService
            .saveNewListening(this.currentSongId)
            .pipe(this.untilThis)
            .subscribe();
        this.playStream(`https://localhost:7284/api/Song/file/${this.currentSongId}`);
    }

    pause() {
        this.audioService.pause();
    }

    play() {
        this.audioService.play();
    }

    stop() {
        this.audioService.stop();
    }

    getSongName() {
        if (!this.currentSongId) {
            return '';
        }
        const index = this.files.findIndex((file) => file.id === this.currentSongId);

        if (index < 0) {
            return '';
        }

        return this.files[index].name;
    }

    next() {
        let index = this.currentSongId ? this.files.findIndex((file) => file.id === this.currentSongId) + 1 : 0;

        if (index > this.files.length - 1) {
            index = 0;
        }

        const file = this.files[index];

        this.openFile(file.id);
    }

    previous() {
        let index = this.currentSongId ? this.files.findIndex((file) => file.id === this.currentSongId) - 1 : 0;

        if (index < 0) {
            index = 0;
        }
        const file = this.files[index];

        this.openFile(file.id);
    }

    onSliderChangeEnd(change: MatSliderChange) {
        // @ts-ignore
        this.audioService.seekTo(change.value);
    }
}
