import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'time',
})
export class TimePipe implements PipeTransform {
    transform(value: string): unknown {
        return value.substring(3, 8);
    }
}
