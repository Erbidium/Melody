import { Injectable } from '@angular/core';
import { IBaseSong } from '@core/models/IBaseSong';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PlayerService {
    filesKey = 'files';

    currentIdKey = 'currentSongId';

    private playerStateSource = new BehaviorSubject<{ id?: number; files: IBaseSong[] }>(this.getPlayerState());

    playerStateEmitted$ = this.playerStateSource.asObservable();

    emitPlayerStateChange(currentSongId: number | undefined, files: IBaseSong[]) {
        this.playerStateSource.next({ id: currentSongId, files });
        if (currentSongId) {
            sessionStorage.setItem(this.currentIdKey, JSON.stringify(currentSongId));
        }
        sessionStorage.setItem(this.filesKey, JSON.stringify(files));
    }

    public getPlayerState(): { id?: number; files: IBaseSong[] } {
        const currentIdString = sessionStorage.getItem(this.currentIdKey);
        const filesString = sessionStorage.getItem(this.filesKey);
        const currentId = currentIdString ? JSON.parse(currentIdString) as number : undefined;
        const files = filesString ? JSON.parse(filesString) as IBaseSong[] : [];

        return { id: currentId, files };
    }
}
