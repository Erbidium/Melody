import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatSliderChange } from '@angular/material/slider';
import { BaseComponent } from '@core/base/base.component';
import { NextSong } from '@core/enums/NextSong';
import { StreamState } from '@core/interfaces/stream-state';
import { IBaseSong } from '@core/models/IBaseSong';
import { AudioService } from '@core/services/audio.service';
import { PlayerService } from '@core/services/player.service';
import { SongService } from '@core/services/song.service';

@Component({
    selector: 'app-player',
    templateUrl: './player.component.html',
    styleUrls: ['./player.component.sass'],
})
export class PlayerComponent extends BaseComponent implements OnInit {
    files: Array<IBaseSong> = [];

    state?: StreamState;

    currentSongIdValue?: number;

    nextSongMode: NextSong = NextSong.NextInOrder;

    constructor(
        public audioService: AudioService,
        public songService: SongService,
        public playerService: PlayerService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.audioService
            .getState()
            .pipe(this.untilThis)
            .subscribe((state) => {
                this.state = state;
                if (this.state?.isFinished && this.currentSongIdValue) {
                    if (this.nextSongMode === NextSong.NextInOrder) {
                        this.next();
                    } else if (this.nextSongMode === NextSong.Same) {
                        this.openFile(this.currentSongIdValue);
                    } else if (this.nextSongMode === NextSong.Random) {
                        this.random();
                    }
                }
            });
        this.playerService
            .playerStateEmitted$
            .pipe(this.untilThis)
            .subscribe((playerState) => {
                const songId = playerState.id;

                if (songId && songId !== this.currentSongIdValue) {
                    this.files = playerState.files;
                    this.openFile(songId);
                } else if (songId === undefined) {
                    this.stop();
                    this.currentSongIdValue = undefined;
                }
            });
    }

    playStream(url: string) {
        this.audioService
            .playStream(url)
            .pipe(this.untilThis)
            .subscribe(() => {});
    }

    openFile(fileId: number) {
        this.currentSongIdValue = fileId;
        this.playerService.emitPlayerStateChange(this.currentSongIdValue, this.files);
        this.audioService.stop();
        this.songService
            .saveNewListening(this.currentSongIdValue)
            .pipe(this.untilThis)
            .subscribe();
        this.playStream(`https://localhost:7284/api/Song/file/${this.currentSongIdValue}`);
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
        if (!this.currentSongIdValue) {
            return '';
        }
        const index = this.files.findIndex((file) => file.id === this.currentSongIdValue);

        if (index < 0) {
            return '';
        }

        return this.files[index].name;
    }

    next() {
        let index = this.currentSongIdValue ? this.files.findIndex((file) => file.id === this.currentSongIdValue) + 1 : 0;

        if (index > this.files.length - 1) {
            index = 0;
        }

        const file = this.files[index];

        this.openFile(file.id);
    }

    random() {
        const index = Math.floor(Math.random() * this.files.length);
        const file = this.files[index];

        this.openFile(file.id);
    }

    previous() {
        let index = this.currentSongIdValue ? this.files.findIndex((file) => file.id === this.currentSongIdValue) - 1 : 0;

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

    setRepeatMode() {
        if (this.nextSongMode === NextSong.Same) {
            this.nextSongMode = NextSong.NextInOrder;
        } else {
            this.nextSongMode = NextSong.Same;
        }
    }

    setRandomMode() {
        if (this.nextSongMode === NextSong.Random) {
            this.nextSongMode = NextSong.NextInOrder;
        } else {
            this.nextSongMode = NextSong.Random;
        }
    }

    isRandomMode() {
        return this.nextSongMode === NextSong.Random;
    }

    isSameMode() {
        return this.nextSongMode === NextSong.Same;
    }
}
