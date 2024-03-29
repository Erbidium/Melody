import { Injectable } from '@angular/core';
import { StreamState } from '@core/interfaces/stream-state';
import * as moment from 'moment';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
export class AudioService {
    private stop$ = new Subject<void>();

    private audioObj = new Audio();

    private state: StreamState = {
        playing: false,
        readableCurrentTime: '',
        readableDuration: '',
        duration: undefined,
        currentTime: undefined,
        canplay: false,
        error: false,
        isFinished: false,
    };

    private stateChange: BehaviorSubject<StreamState> = new BehaviorSubject(
        this.state,
    );

    audioEvents = [
        'ended',
        'error',
        'play',
        'playing',
        'pause',
        'timeupdate',
        'canplay',
        'loadedmetadata',
        'loadstart',
    ];

    private addEvents(obj: HTMLAudioElement, events: string[], handler: (event: Event) => void) {
        events.forEach(event => {
            obj.addEventListener(event, handler);
        });
    }

    private removeEvents(obj: HTMLAudioElement, events: string[], handler: (event: Event) => void) {
        events.forEach(event => {
            obj.removeEventListener(event, handler);
        });
    }

    playStream(url: string) {
        return this.streamObservable(url).pipe(takeUntil(this.stop$));
    }

    play() {
        this.audioObj.play();
    }

    pause() {
        this.audioObj.pause();
    }

    stop() {
        this.stop$.next();
    }

    seekTo(seconds: number) {
        this.audioObj.currentTime = seconds;
    }

    formatTime(time: number, format: string = 'HH:mm:ss') {
        const momentTime = time * 1000;

        return moment.utc(momentTime).format(format);
    }

    private updateStateEvents(event: Event): void {
        switch (event.type) {
            case 'canplay':
                this.state.duration = this.audioObj.duration;
                this.state.readableDuration = this.formatTime(this.state.duration);
                this.state.canplay = true;
                break;
            case 'playing':
                this.state.playing = true;
                break;
            case 'pause':
                this.state.playing = false;
                break;
            case 'timeupdate':
                this.state.currentTime = this.audioObj.currentTime;
                this.state.readableCurrentTime = this.formatTime(
                    this.state.currentTime,
                );
                break;
            case 'error':
                this.resetState();
                this.state.error = true;
                break;
            case 'ended':
                this.state.isFinished = true;
                break;
            default:
                this.state.playing = false;
                break;
        }
        this.stateChange.next(this.state);
    }

    private resetState() {
        this.state = {
            playing: false,
            readableCurrentTime: '',
            readableDuration: '',
            duration: undefined,
            currentTime: undefined,
            canplay: false,
            error: false,
            isFinished: false,
        };
    }

    getState(): Observable<StreamState> {
        return this.stateChange.asObservable();
    }

    private streamObservable(url: string) {
        return new Observable(observer => {
            this.audioObj.src = url;
            this.audioObj.load();
            this.audioObj.autoplay = true;
            const res = this.audioObj.play();

            res.catch(e => e);

            const handler = (event: Event) => {
                this.updateStateEvents(event);
                observer.next(event);
            };

            this.addEvents(this.audioObj, this.audioEvents, handler);

            return () => {
                this.audioObj.pause();
                this.audioObj.currentTime = 0;
                this.removeEvents(this.audioObj, this.audioEvents, handler);
                this.resetState();
                this.stateChange.next(this.state);
            };
        });
    }
}
