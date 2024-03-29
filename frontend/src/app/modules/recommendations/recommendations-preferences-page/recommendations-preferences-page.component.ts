import { HttpErrorResponse } from '@angular/common/http';
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
import { switchMap } from 'rxjs/operators';

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
        this.songService.getAllGenres()
            .pipe(this.untilThis)
            .pipe(
                switchMap((genres: IGenre[]) => {
                    this.genres = genres;

                    return this.userService.getUserRecommendationPreferences();
                }),
            )
            .subscribe({
                next: (preferences) => {
                    this.uploadForm.patchValue({
                        author: preferences.authorName,
                        startYear: preferences.startYear?.toString(),
                        endYear: preferences.endYear?.toString(),
                        averageDurationInMinutes: preferences.averageDurationInMinutes?.toString(),
                    });
                    this.selectedGenre = this.genres.find((g) => g.id === preferences.genreId);
                },
                error: (e) => {
                    if (!(e instanceof HttpErrorResponse && e.status === 404)) {
                        this.notificationService.showErrorMessage('Трапилася помилка');
                    }
                },
            });
    }

    savePreferences() {
        if (this.selectedGenre && this.uploadForm.valid) {
            let startYearValue = this.uploadForm.value.startYear;
            let endYearValue = this.uploadForm.value.endYear;
            let averageDurationInMinutesValue = this.uploadForm.value.averageDurationInMinutes;

            if (startYearValue === '') {
                startYearValue = undefined;
            }
            if (endYearValue === '') {
                endYearValue = undefined;
            }
            if (averageDurationInMinutesValue === '') {
                averageDurationInMinutesValue = undefined;
            }
            const preferences: IPreferences = {
                authorName: this.uploadForm.value.author ?? undefined,
                startYear: (startYearValue as unknown as number) ?? undefined,
                endYear: (endYearValue as unknown as number) ?? undefined,
                genreId: this.selectedGenre.id,
                averageDurationInMinutes:
                    (averageDurationInMinutesValue as unknown as number) ?? undefined,
            };

            this.userService
                .saveUserRecommendationPreferences(preferences)
                .pipe(this.untilThis)
                .subscribe({
                    next: () => {
                        this.notificationService.showSuccessMessage('Вподобання були успішно завантажені');
                    },
                    error: () => this.notificationService.showErrorMessage('Трапилася помилка'),
                });
        }
    }
}
