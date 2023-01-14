import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class CustomValidators {
    static yearsRangeCorrect: ValidatorFn = (form: AbstractControl<any, any>): ValidationErrors | null => {
        const { startYear } = form.value;
        const { endYear } = form.value;
        const invalidYears = startYear && endYear && startYear > endYear;

        return !invalidYears ? null : { incorrectYears: true };
    };
}
