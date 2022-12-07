import { ISongFromPlaylist } from '@core/models/ISongFromPlaylist';

export interface IPlaylist {
    id: number;
    name: string;
    authorId: string;
    songs: ISongFromPlaylist[];
}
