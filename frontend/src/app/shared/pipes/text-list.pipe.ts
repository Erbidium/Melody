import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'textList',
})
export class TextListPipe implements PipeTransform {
    transform(value: string[]): unknown {
        if (value.length <= 3) {
            return value.join(', ');
        }

        return `${value.slice(0, 3).join(', ')}...`;
    }
}
