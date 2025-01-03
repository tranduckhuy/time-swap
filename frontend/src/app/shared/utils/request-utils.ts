import { HttpParams } from "@angular/common/http";

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