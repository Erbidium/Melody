import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@core/services/auth.service';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';

const TOKEN_HEADER_KEY = 'Authorization';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private isRefreshing = false;

    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    // eslint-disable-next-line no-empty-function
    constructor(private authService: AuthService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<Object>> {
        let authReq = req;
        const token = this.authService.getAccessToken();

        if (token != null) {
            authReq = this.addTokenHeader(req, token);
        }

        return next.handle(authReq).pipe(
            catchError((error) => {
                if (
                    error instanceof HttpErrorResponse &&
                    !authReq.url.includes('api/login') &&
                    error.status === 401
                ) {
                    return this.handle401Error(authReq, next);
                }

                return throwError(error);
            }),
        );
    }

    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            return this.authService.refreshToken().pipe(
                switchMap((token: any) => {
                    this.isRefreshing = false;

                    localStorage.setItem('access-token', token.accessToken);
                    this.refreshTokenSubject.next(token.accessToken);

                    return next.handle(this.addTokenHeader(request, token.accessToken));
                }),
                catchError((err) => {
                    this.isRefreshing = false;

                    this.authService.signOut();

                    return throwError(err);
                }),
            );
        }

        return this.refreshTokenSubject.pipe(
            filter((token) => token !== null),
            take(1),
            switchMap((token) => next.handle(this.addTokenHeader(request, token))),
        );
    }

    private addTokenHeader(request: HttpRequest<any>, token: string) {
        return request.clone({ headers: request.headers.set(TOKEN_HEADER_KEY, `Bearer ${token}`) });
    }
}
