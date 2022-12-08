import { Component, Input } from '@angular/core';
import { ISongTable } from '@core/models/ISongTable';

@Component({
    selector: 'app-songs-table',
    templateUrl: './songs-table.component.html',
    styleUrls: ['./songs-table.component.sass'],
})
export class SongsTableComponent {
    @Input() dataSource: ISongTable[] = [];

    @Input() clickHeartButtonHandler: ((id: number, event: MouseEvent) => void) | undefined;

    @Input() clickRemoveButtonHandler: ((id: number, event: MouseEvent) => void) | undefined;

    @Input() rowClickHandler: ((songId: number) => void) | undefined;

    @Input() columnsToDisplay: string[] = [];
}
