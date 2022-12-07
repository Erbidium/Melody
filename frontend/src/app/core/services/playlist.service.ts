import { Injectable } from '@angular/core';
import { IPlaylist } from '@core/models/IPlaylist';
import { IPlaylistWithPerformers } from '@core/models/IPlaylistWithPerformers';
import { HttpInternalService } from '@core/services/http-internal-service';
import {ISong} from "@core/models/ISong";

@Injectable({ providedIn: 'root' })
export class PlaylistService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public createPlaylist(name: string, songIds: number[]) {
        return this.httpService.postRequest('/api/playlist/', { name, songIds });
    }

    public getPlaylistsCreatedByUser() {
        return this.httpService.getRequest<IPlaylistWithPerformers[]>('/api/playlist/created');
    }

    public getPlaylistById(id: number) {
        return this.httpService.getRequest<IPlaylist>(`/api/playlist/${id}`);
    }

    public removeSongFromPlaylist(songId: number, playlistId: number) {
        return this.httpService.deleteRequest(`/api/playlist/${playlistId}/song/${songId}`);
    }

    public deletePlaylist(id: number) {
        return this.httpService.deleteRequest(`/api/playlist/${id}`);
    }

    public getSongsToAddToPlaylist(id: number) {
        return this.httpService.getRequest<ISong[]>(`/api/playlist/${id}/new-songs-to-add`);
    }

    public addSongs(id: number, newSongIds: number[]) {
        return this.httpService.putRequest(`/api/playlist/${id}`, { NewSongIds: newSongIds });
    }
}
