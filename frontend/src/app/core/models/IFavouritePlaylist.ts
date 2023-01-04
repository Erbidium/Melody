import { ISongFromPlaylist } from '@core/models/ISongFromPlaylist';

export interface IFavouritePlaylist {
    id: number;
    name: string;
    authorId: number;
    isFavourite: boolean;
    songs: ISongFromPlaylist[];
}
