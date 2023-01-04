import { IBaseSong } from '@core/models/IBaseSong';
import { IGenre } from '@core/models/IGenre';

export interface ISong extends IBaseSong {
    userId: number;
    authorName: string;
    year: number;
    uploadedAt: Date;
    genreId: number;
    genre?: IGenre;
}
