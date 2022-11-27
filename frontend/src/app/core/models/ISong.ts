import {IGenre} from "@core/models/IGenre";

export interface ISong {
    id: number;
    userId: number;
    name: string;
    path: string;
    authorName: string;
    year: number;
    sizeBytes: number;
    uploadedAt: Date;
    genreId: number;
    duration: string;
    genre?: IGenre;
}
