import { ISong } from '@core/models/ISong';

export interface IPlaylist {
    id: number;
    name: string;
    authorId: string;
    songs: ISong[];
}
