import { Injectable } from '@angular/core';
import { IGenre } from '@core/models/IGenre';
import { ISong } from '@core/models/ISong';
import { HttpInternalService } from '@core/services/http-internal-service';

@Injectable({ providedIn: 'root' })
export class PlaylistService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public createPlaylist(name: string, songIds: number[]) {
        return this.httpService.postRequest('/api/playlist/', { name, songIds });
    }
}
