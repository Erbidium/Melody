import { IBaseSong } from '@core/models/IBaseSong';
import { IGenre } from '@core/models/IGenre';

export interface ISongFromPlaylist extends IBaseSong {
    authorName: string;
    uploadedAt: Date;
    genreId: number;
    genre?: IGenre;
    isFavourite: boolean;
}
