import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class InfiniteScrollingService {
    private intersectionSubject = new BehaviorSubject<boolean>(false);

    public intersectionOptions = {
        root: null,
        rootMargin: '0px',
        threshold: [0, 0.5, 1],
    };

    private observer = new IntersectionObserver(this.intersectionCallback.bind(this), this.intersectionOptions);

    getObservable() {
        return this.intersectionSubject.asObservable();
    }

    intersectionCallback(entries: IntersectionObserverEntry[], _: IntersectionObserver) {
        entries.forEach((entry: IntersectionObserverEntry) => {
            this.intersectionSubject.next(entry.intersectionRatio === 1);
        });
    }

    setObserver() {
        return this.observer;
    }
}
