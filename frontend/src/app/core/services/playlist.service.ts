import { Injectable } from '@angular/core';
import { IFavouritePlaylist } from '@core/models/IFavouritePlaylist';
import { IFavouritePlaylistWithPerformers } from '@core/models/IFavouritePlaylistWithPerformers';
import { ISong } from '@core/models/ISong';
import { HttpInternalService } from '@core/services/http-internal-service';

@Injectable({ providedIn: 'root' })
export class PlaylistService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    createPlaylist(name: string, songIds: number[]) {
        return this.httpService.postRequest('/api/playlist/', { name, songIds });
    }

    getPlaylistsCreatedByUser() {
        return this.httpService.getRequest<IFavouritePlaylistWithPerformers[]>('/api/playlist/created');
    }

    getFavouritePlaylists() {
        return this.httpService.getRequest<IFavouritePlaylistWithPerformers[]>('/api/playlist/favourite');
    }

    getPlaylistById(id: number) {
        return this.httpService.getRequest<IFavouritePlaylist>(`/api/playlist/${id}`);
    }

    removeSongFromPlaylist(songId: number, playlistId: number) {
        return this.httpService.deleteRequest(`/api/playlist/${playlistId}/song/${songId}`);
    }

    deletePlaylist(id: number) {
        return this.httpService.deleteRequest(`/api/playlist/${id}`);
    }

    getSongsToAddToPlaylist(id: number) {
        return this.httpService.getRequest<ISong[]>(`/api/playlist/${id}/new-songs-to-add`);
    }

    addSongs(id: number, newSongIds: number[]) {
        return this.httpService.putRequest(`/api/playlist/${id}`, { NewSongIds: newSongIds });
    }

    removePlaylistFromUserFavourites(id: number) {
        return this.httpService.deleteRequest(`/api/playlist/favourite/${id}`);
    }

    setPlaylistStatus(id: number, isLiked: boolean) {
        return this.httpService.patchRequest(`/api/playlist/${id}/like`, { isLiked });
    }
}
