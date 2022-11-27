import {Component, EventEmitter, Input, Output} from '@angular/core';
import { MatSliderChange } from '@angular/material/slider';
import { StreamState } from '@core/interfaces/stream-state';
import { ISong } from '@core/models/ISong';
import { AudioService } from '@core/services/audio.service';

@Component({
    selector: 'app-player',
    templateUrl: './player.component.html',
    styleUrls: ['./player.component.sass'],
})
export class PlayerComponent {
    @Input() files: Array<ISong> = [];

    // @ts-ignore
    state: StreamState;

    songId?: number;

    constructor(public audioService: AudioService) {
        // listen to stream state
        this.audioService.getState().subscribe((state) => {
            this.state = state;
        });
    }

    playStream(url: string) {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars,no-unused-vars
        this.audioService.playStream(url).subscribe((events) => {
            // listening for fun here
        });
    }

    @Input() set currentSongIdSetter(songId: number | undefined) {
        if (songId && songId !== this.songId) {
            this.songId = songId;
            this.openFile(songId);
        }
    }

    currentSongId?: number;

    openFile(fileId: number) {
        this.currentSongId = fileId;
        this.audioService.stop();
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

    next() {
        const index = this.currentSongId ? this.files.findIndex(file => file.id === this.currentSongId) + 1 : 0;
        const file = this.files[index];

        this.openFile(file.id);
    }

    previous() {
        const index = this.currentSongId ? this.files.findIndex(file => file.id === this.currentSongId) - 1 : 0;
        const file = this.files[index];

        this.openFile(file.id);
    }

    isFirstPlaying(): boolean {
        return this.currentSongId !== undefined && this.files.findIndex(file => file.id === this.currentSongId) === 0;
    }

    isLastPlaying(): boolean {
        return this.currentSongId !== undefined && this.files.findIndex(file => file.id === this.currentSongId) === this.files.length - 1;
    }

    onSliderChangeEnd(change: MatSliderChange) {
        // @ts-ignore
        this.audioService.seekTo(change.value);
    }
}
