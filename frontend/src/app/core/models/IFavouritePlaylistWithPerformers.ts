export interface IFavouritePlaylistWithPerformers {
    id: number;
    name: string;
    authorId: string;
    isFavourite: boolean;
    performersNames: string[];
}
