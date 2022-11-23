import { Injectable } from '@angular/core';
import { IGenre } from '@core/models/IGenre';
import { ISong } from '@core/models/ISong';
import { HttpInternalService } from '@core/services/http-internal-service';

@Injectable({ providedIn: 'root' })
export class SongService {
    // eslint-disable-next-line no-empty-function
    constructor(private httpService: HttpInternalService) {}

    public getAll() {
        return this.httpService.getRequest<IGenre[]>('/api/song/genres');
    }

    public createSong(formData: FormData) {
        return this.httpService.postRequest<ISong>('/api/song', formData);
    }
}
