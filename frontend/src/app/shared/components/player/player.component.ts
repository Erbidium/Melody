import { Component } from '@angular/core';
import { MatSliderChange } from '@angular/material/slider';
import { StreamState } from '@core/interfaces/stream-state';
import { AudioService } from '@core/services/audio.service';
import { CloudService } from '@core/services/cloud.service';

@Component({
    selector: 'app-player',
    templateUrl: './player.component.html',
    styleUrls: ['./player.component.sass'],
})
export class PlayerComponent {
    files: Array<any> = [
        { name: 'First Song', artist: 'Inder' },
        { name: 'Second Song', artist: 'You' },
    ];

    currentFile: any = {};

    // @ts-ignore
    state: StreamState;

    constructor(
        public audioService: AudioService,
        public cloudService: CloudService,
    ) {
        // get media files
        cloudService.getFiles().subscribe(files => {
            this.files = files;
        });

        // listen to stream state
        this.audioService.getState().subscribe(state => {
            this.state = state;
        });
    }

    playStream(url: string) {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars,no-unused-vars
        this.audioService.playStream(url).subscribe(events => {
            // listening for fun here
        });
    }

    // @ts-ignore
    openFile(file, index: number) {
        this.currentFile = { index, file };
        this.audioService.stop();
        this.playStream(file.url);
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
        const index = this.currentFile.index + 1;
        const file = this.files[index];

        this.openFile(file, index);
    }

    previous() {
        const index = this.currentFile.index - 1;
        const file = this.files[index];

        this.openFile(file, index);
    }

    isFirstPlaying() {
        return this.currentFile.index === 0;
    }

    isLastPlaying() {
        return this.currentFile.index === this.files.length - 1;
    }

    onSliderChangeEnd(change: MatSliderChange) {
        // @ts-ignore
        this.audioService.seekTo(change.value);
    }
}
