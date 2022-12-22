import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@core/base/base.component';
import { columnsToDisplayWithRemoveColumn } from '@core/helpers/columns-to-display-helper';
import { ISong } from '@core/models/ISong';
import { InfiniteScrollingService } from '@core/services/infinite-scrolling.service';
import { PlayerService } from '@core/services/player.service';
import { SongService } from '@core/services/song.service';
import { SpinnerOverlayService } from '@core/services/spinner-overlay.service';
import { switchMap } from 'rxjs/operators';

@Component({
    selector: 'app-user-uploads-page',
    templateUrl: './user-uploads-page.component.html',
    styleUrls: ['./user-uploads-page.component.sass'],
})
export class UserUploadsPageComponent extends BaseComponent implements OnInit {
    uploadedSongs: ISong[] = [];

    columnsToDisplay = columnsToDisplayWithRemoveColumn;

    currentSongIdForMusicPlayer?: number;

    page = 1;

    pageSize = 10;

    constructor(
        private songService: SongService,
        private playerService: PlayerService,
        private spinnerOverlayService: SpinnerOverlayService,
        private scrollService: InfiniteScrollingService,
    ) {
        super();
    }

    ngOnInit() {
        this.loadSongsUploadedByUser(this.page, this.pageSize);
        this.playerService
            .playerStateEmitted$
            .pipe(this.untilThis)
            .subscribe(state => {
                this.currentSongIdForMusicPlayer = state.id;
            });

        this.scrollService
            .getObservable()
            .pipe(this.untilThis)
            .subscribe((status: boolean) => {
                if (status) {
                    this.loadSongsUploadedByUser(++this.page, this.pageSize);
                }
            });
    }

    loadSongsUploadedByUser(page: number, pageSize: number, updateAllData = false) {
        this.spinnerOverlayService.show();
        this.songService
            .getSongsUploadedByUser(page, pageSize)
            .pipe(this.untilThis)
            .subscribe((response) => {
                this.spinnerOverlayService.hide();

                this.uploadedSongs = updateAllData ? response : this.uploadedSongs.concat(response);

                const clear = setInterval(() => {
                    const target = document.querySelector(`#target${page * pageSize - 1}`);

                    if (target) {
                        clearInterval(clear);
                        this.scrollService.setObserver().observe(target);
                    }
                }, 2000);
            });
    }

    selectSong(songId: number) {
        this.playerService.emitPlayerStateChange(songId, this.uploadedSongs);
    }

    deleteSong(id: number, event: MouseEvent) {
        event.stopPropagation();
        this.songService
            .deleteSong(id)
            .pipe(switchMap(async () => {
                this.uploadedSongs = [];
                this.loadSongsUploadedByUser(1, this.page * this.pageSize, true);
            }))
            .subscribe(() => {
                this.currentSongIdForMusicPlayer = undefined;
                this.playerService.emitPlayerStateChange(undefined, []);
            });
    }
}
