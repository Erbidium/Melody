import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
    selector: '[appHorizontalScroll]',
})
export class HorizontalScrollDirective {
    // eslint-disable-next-line no-empty-function
    constructor(private element: ElementRef) {}

    @HostListener('wheel', ['$event'])
    public onScroll(event: WheelEvent) {
        this.element.nativeElement.scrollLeft += event.deltaY;
    }
}
