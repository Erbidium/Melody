import { Component, Input } from '@angular/core';
import { MatSliderChange } from '@angular/material/slider';
import { StreamState } from '@core/interfaces/stream-state';
import { ISong } from '@core/models/ISong';
import { AudioService } from '@core/services/audio.service';
import { BaseComponent } from '@core/base/base.component';

@Component({
    selector: 'app-player',
    templateUrl: './player.component.html',
    styleUrls: ['./player.component.sass'],
})
export class PlayerComponent extends BaseComponent {
    @Input() files: Array<ISong> = [];

    state?: StreamState;

    currentSongId?: number;

    constructor(public audioService: AudioService) {
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
        const index = this.currentSongId ? this.files.findIndex((file) => file.id === this.currentSongId) + 1 : 0;
        const file = this.files[index];

        this.openFile(file.id);
    }

    previous() {
        const index = this.currentSongId ? this.files.findIndex((file) => file.id === this.currentSongId) - 1 : 0;
        const file = this.files[index];

        this.openFile(file.id);
    }

    isFirstPlaying(): boolean {
        return this.currentSongId !== undefined && this.files.findIndex((file) => file.id === this.currentSongId) === 0;
    }

    isLastPlaying(): boolean {
        return (
            this.currentSongId !== undefined &&
            this.files.findIndex((file) => file.id === this.currentSongId) === this.files.length - 1
        );
    }

    onSliderChangeEnd(change: MatSliderChange) {
        // @ts-ignore
        this.audioService.seekTo(change.value);
    }
}
