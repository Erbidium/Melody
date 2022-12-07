import { Component, Input, OnInit } from '@angular/core';
import { ISong } from '@core/models/ISong';
import { ISongFromPlaylist } from '@core/models/ISongFromPlaylist';

@Component({
    selector: 'app-songs-table',
    templateUrl: './songs-table.component.html',
    styleUrls: ['./songs-table.component.sass'],
})
export class SongsTableComponent {
    @Input() dataSource: ISongFromPlaylist[] = [];

    @Input() clickHeartButtonHandler: ((id: number, event: MouseEvent) => void) | undefined;

    @Input() rowClickHandler: ((songId: number) => void) | undefined;

    @Input() columnsToDisplay: string[] = [];
}
