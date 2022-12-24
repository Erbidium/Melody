import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class InfiniteScrollingService {
    private intersectionSubject = new BehaviorSubject<{ isIntersecting: boolean, id: string } >({ isIntersecting: false, id: '' });

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
            this.intersectionSubject.next({ isIntersecting: entry.intersectionRatio === 1, id: entry.target.id });
        });
    }

    setObserver() {
        return this.observer;
    }
}
