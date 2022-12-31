import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BaseComponent } from '@core/base/base.component';
import { IGenre } from '@core/models/IGenre';
import { IPreferences } from '@core/models/IPreferences';
import { SongService } from '@core/services/song.service';
import { UserService } from '@core/services/user.service';
import {NotificationService} from "@core/services/notification.service";

@Component({
    selector: 'app-recommendations-preferences-page',
    templateUrl: './recommendations-preferences-page.component.html',
    styleUrls: ['./recommendations-preferences-page.component.sass'],
})
export class RecommendationsPreferencesPageComponent extends BaseComponent implements OnInit {
    genres: IGenre[] = [];

    selectedGenre?: IGenre;

    uploadForm = new FormGroup({
        author: new FormControl('', {
            updateOn: 'blur',
        }),
        startYear: new FormControl('', {
            updateOn: 'blur',
        }),
        endYear: new FormControl('', {
            updateOn: 'blur',
        }),
        averageDurationInMinutes: new FormControl('', {
            updateOn: 'blur',
        }),
    });

    constructor(
        private songService: SongService,
        private userService: UserService,
        private notificationService: NotificationService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.songService
            .getAllGenres()
            .pipe(this.untilThis)
            .subscribe((resp) => {
                this.genres = resp;
            });
    }

    savePreferences() {
        if (this.selectedGenre) {
            const preferences: IPreferences = {
                authorName: this.uploadForm.value.author ?? undefined,
                startYear: (this.uploadForm.value.startYear as unknown as number) ?? undefined,
                endYear: (this.uploadForm.value.endYear as unknown as number) ?? undefined,
                genreId: this.selectedGenre.id,
                averageDurationInMinutes: (this.uploadForm.value.averageDurationInMinutes as unknown as number) ?? undefined,
            };
            console.log(preferences);
            this.userService.saveUserRecommendationPreferences(preferences)
                .pipe(this.untilThis)
                .subscribe({
                    next: () => {
                        this.notificationService.showSuccessMessage('Preferences were successfully uploaded');
                        this.uploadForm.reset();
                        this.selectedGenre = undefined;
                    },
                    error: () => this.notificationService.showErrorMessage('Error occurred'),
                });
        }
    }
}
