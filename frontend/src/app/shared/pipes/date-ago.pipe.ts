import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'dateAgo',
    pure: true,
})
export class DateAgoPipe implements PipeTransform {
    // eslint-disable-next-line no-unused-vars,@typescript-eslint/no-unused-vars
    transform(value: any, args?: any): any {
        if (value) {
            const seconds = Math.floor((+new Date() - +new Date(value)) / 1000);

            if (seconds < 29) { return 'Just now'; }
            const intervals = {
                year: 31536000,
                month: 2592000,
                week: 604800,
                day: 86400,
                hour: 3600,
                minute: 60,
                second: 1,
            };
            let counter;

            // eslint-disable-next-line guard-for-in,no-restricted-syntax
            for (const i in intervals) {
                // @ts-ignore
                counter = Math.floor(seconds / intervals[i]);
                if (counter > 0) {
                    if (counter === 1) {
                        return `${counter} ${i} ago`;
                    }

                    return `${counter} ${i}s ago`;
                }
            }
        }

        return value;
    }
}
