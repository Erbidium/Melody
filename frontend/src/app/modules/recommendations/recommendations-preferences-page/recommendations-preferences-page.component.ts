import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BaseComponent } from '@core/base/base.component';
import { IGenre } from '@core/models/IGenre';
import { IPreferences } from '@core/models/IPreferences';
import { NotificationService } from '@core/services/notification.service';
import { SongService } from '@core/services/song.service';
import { UserService } from '@core/services/user.service';
import { CustomErrorStateMatcher } from '@modules/recommendations/validators/custom-error-state-matcher';
import { CustomValidators } from '@modules/recommendations/validators/custom-validators';
import { forkJoin } from 'rxjs';

@Component({
    selector: 'app-recommendations-preferences-page',
    templateUrl: './recommendations-preferences-page.component.html',
    styleUrls: ['./recommendations-preferences-page.component.sass'],
})
export class RecommendationsPreferencesPageComponent extends BaseComponent implements OnInit {
    genres: IGenre[] = [];

    selectedGenre?: IGenre;

    matcher = new CustomErrorStateMatcher();

    uploadForm = new FormGroup(
        {
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
        },
        {
            validators: CustomValidators.yearsRangeCorrect,
            updateOn: 'blur',
        },
    );

    constructor(
        private songService: SongService,
        private userService: UserService,
        private notificationService: NotificationService,
    ) {
        super();
    }

    ngOnInit(): void {
        forkJoin([
            this.songService.getAllGenres(),
            this.userService.getUserRecommendationPreferences(),
        ])
            .pipe(this.untilThis)
            .subscribe((resp) => {
                [this.genres] = resp;
                const preferences = resp[1];

                this.uploadForm.patchValue({
                    author: preferences.authorName,
                    startYear: preferences.startYear?.toString(),
                    endYear: preferences.endYear?.toString(),
                    averageDurationInMinutes: preferences.averageDurationInMinutes?.toString(),
                });
                this.selectedGenre = this.genres.find(g => g.id === preferences.genreId);
            });
    }

    savePreferences() {
        if (this.selectedGenre && this.uploadForm.valid) {
            const preferences: IPreferences = {
                authorName: this.uploadForm.value.author ?? undefined,
                startYear: (this.uploadForm.value.startYear as unknown as number) ?? undefined,
                endYear: (this.uploadForm.value.endYear as unknown as number) ?? undefined,
                genreId: this.selectedGenre.id,
                averageDurationInMinutes: (this.uploadForm.value.averageDurationInMinutes as unknown as number) ?? undefined,
            };

            this.userService.saveUserRecommendationPreferences(preferences)
                .pipe(this.untilThis)
                .subscribe({
                    next: () => {
                        this.notificationService.showSuccessMessage('Preferences were successfully uploaded');
                    },
                    error: () => this.notificationService.showErrorMessage('Error occurred'),
                });
        }
    }
}
