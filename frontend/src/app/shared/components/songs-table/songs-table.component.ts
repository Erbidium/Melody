import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ISongTable } from '@core/models/ISongTable';

@Component({
    selector: 'app-songs-table',
    templateUrl: './songs-table.component.html',
    styleUrls: ['./songs-table.component.sass'],
})
export class SongsTableComponent {
    @Input() dataSource: ISongTable[] = [];

    @Input() columnsToDisplay: string[] = [];

    @Input() heartButtonForLikes: boolean = false;

    @Output() heartButtonClickEvent = new EventEmitter<{ id: number, event: MouseEvent }>();

    @Output() removeButtonClickEvent = new EventEmitter<{ id: number, event: MouseEvent }>();

    @Output() rowClickEvent = new EventEmitter<number>();

    @Input() currentSongSelected?: number;

    getMatIconText(song: ISongTable) {
        if (!this.heartButtonForLikes) {
            return 'favorite';
        }
        const favouriteSong = song as unknown as { isFavourite: boolean };

        return favouriteSong.isFavourite ? 'favorite' : 'favorite_border';
    }
}
