import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class HttpInternalService {
    private baseUrl: string = environment.coreUrl;

    private headers = new HttpHeaders();

    // eslint-disable-next-line no-empty-function
    constructor(private http: HttpClient) {}

    getRequest<T>(url: string, httpParams?: any): Observable<T> {
        return this.http.get<T>(this.buildUrl(url), { headers: this.getHeaders(), params: httpParams });
    }

    postRequest<T>(url: string, payload: object, httpParams?: any): Observable<T> {
        return this.http.post<T>(this.buildUrl(url), payload, { headers: this.getHeaders(), params: httpParams, withCredentials: true });
    }

    putRequest<T>(url: string, payload: object, httpParams?: any): Observable<T> {
        return this.http.put<T>(this.buildUrl(url), payload, { headers: this.getHeaders(), params: httpParams });
    }

    patchRequest<T>(url: string, payload: object, httpParams?: any): Observable<T> {
        return this.http.patch<T>(this.buildUrl(url), payload, { headers: this.getHeaders(), params: httpParams });
    }

    deleteRequest<T>(url: string, httpParams?: any): Observable<T> {
        return this.http.delete<T>(this.buildUrl(url), { headers: this.getHeaders(), params: httpParams });
    }

    private getHeaders(): HttpHeaders {
        return this.headers;
    }

    private buildUrl(url: string): string {
        if (url.startsWith('http://') || url.startsWith('https://')) {
            return url;
        }

        return this.baseUrl + url;
    }
}
