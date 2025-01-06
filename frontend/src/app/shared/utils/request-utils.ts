import { HttpErrorResponse, HttpParams, HttpRequest } from "@angular/common/http";
import { Router } from "@angular/router";
import { Observable, throwError, timer } from "rxjs";

/**
 * Converts an object of key-value pairs into an HttpParams instance.
 * This is useful when adding query parameters to an HTTP request in Angular.
 *
 * @param params - An object where each key-value pair will be converted into query parameters.
 * @returns An instance of HttpParams with the provided key-value pairs.
 */
export function createHttpParams(params: Record<string, any>): HttpParams {
    let httpParams = new HttpParams();
    Object.keys(params).forEach(key => {
        const value = params[key];
        if (value !== null && value !== undefined) {
        httpParams = httpParams.append(key, value.toString());
        }
    });
    return httpParams;
}

/**
 * Adds an Authorization header with the provided token to an HTTP request.
 *
 * @param req - The original HTTP request.
 * @param token - The access token to include in the Authorization header.
 * @returns A new HTTP request with the Authorization header added.
 */
export function addAuthHeader(req: HttpRequest<unknown>, token: string | null): HttpRequest<unknown> {
    return req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token ?? ''}`)
    });
};

/**
 * Handles unauthorized HTTP errors (401) by redirecting the user to the login page.
 *
 * @param router - The Angular Router instance used for navigation.
 * @returns A function that processes the error and navigates to the login page if the status is 401.
 */
export function handleUnauthorizedError(router: Router) {
    return (error: HttpErrorResponse) => {
        if (error.status === 401) {
        router.navigate(['/auth/login']);
        }
        return throwError(() => error);
    };
};
  
/**
 * A retry strategy that implements exponential backoff for network errors or 5xx server errors.
 *
 * @param error - The error object to evaluate.
 * @param retryCount - The current retry attempt number.
 * @returns An Observable that delays the retry based on the retry strategy or throws the error.
 */
export function retryStrategy(error: any, retryCount: number): Observable<any> {
    const delayMs = Math.min(1000 * Math.pow(2, retryCount), 10000);

    // ? Only retry on network errors or 5xx internal server errors
    if (error instanceof HttpErrorResponse) {
        if (error.status === 0 || (error.status >= 500 && error.status < 600)) {
            return timer(delayMs);
        }
    }

    return throwError(() => error);
};
  
/**
 * Checks if a URL is public and does not require authentication.
 *
 * @param url - The URL to check.
 * @returns A boolean indicating whether the URL is public.
 */
export function isPublicPath(url: string): boolean {
    const publicPaths = new Set<string>([
        '/auth/login',
        '/auth/register',
        '/jobs',
        // ? Can add more endpoints here
    ]);

    return Array.from(publicPaths).some(path => url.includes(path));
};