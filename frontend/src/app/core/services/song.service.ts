import { Injectable } from '@angular/core';
import { IGenre } from '@core/models/IGenre';
import { ISong } from '@core/models/ISong';
import { HttpInternalService } from '@core/services/http-internal-service';

@Injectable({ providedIn: 'root' })
export class SongService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    getSongsUploadedByUser(page: number) {
        return this.httpService.getRequest<ISong[]>('/api/song/', { page });
    }

    getFavouriteAndUploadedUserSongs() {
        return this.httpService.getRequest<ISong[]>('/api/song/favourite-and-uploaded');
    }

    getAllGenres() {
        return this.httpService.getRequest<IGenre[]>('/api/song/genres');
    }

    createSong(formData: FormData) {
        return this.httpService.postRequest<ISong>('/api/song', formData);
    }

    deleteSong(id: number) {
        return this.httpService.deleteRequest(`/api/song/${id}`);
    }

    deleteSongByAdministrator(id: number) {
        return this.httpService.deleteRequest(`/api/song/admin/${id}`);
    }

    getFavouriteUserSongs() {
        return this.httpService.getRequest<ISong[]>('/api/song/favourite');
    }

    removeSongFromUserFavourites(id: number) {
        return this.httpService.deleteRequest(`/api/song/favourite/${id}`);
    }

    setSongStatus(id: number, isLiked: boolean) {
        return this.httpService.patchRequest(`/api/song/${id}/like`, { isLiked });
    }

    saveNewListening(id: number) {
        return this.httpService.postRequest('/api/song/new-listening', { SongId: id });
    }

    getAllSongs() {
        return this.httpService.getRequest<ISong[]>('/api/song/all');
    }
}
