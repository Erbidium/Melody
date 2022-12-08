import { Injectable } from '@angular/core';
import { IGenre } from '@core/models/IGenre';
import { ISong } from '@core/models/ISong';
import { HttpInternalService } from '@core/services/http-internal-service';

@Injectable({ providedIn: 'root' })
export class SongService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public getSongsUploadedByUser() {
        return this.httpService.getRequest<ISong[]>('/api/song/');
    }

    public getAllGenres() {
        return this.httpService.getRequest<IGenre[]>('/api/song/genres');
    }

    public createSong(formData: FormData) {
        return this.httpService.postRequest<ISong>('/api/song', formData);
    }

    public deleteSong(id: number) {
        return this.httpService.deleteRequest(`/api/song/${id}`);
    }

    public getFavouriteUserSongs() {
        return this.httpService.getRequest<ISong[]>('/api/song/favourite');
    }

    public removeSongFromUserFavourites(id: number) {
        return this.httpService.deleteRequest(`/api/song/favourite/${id}`);
    }

    public setSongStatus(id: number, isLiked: boolean) {
        return this.httpService.patchRequest(`/api/song/${id}/like`, { isLiked });
    }
}
