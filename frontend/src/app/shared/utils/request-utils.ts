import { HttpParams } from "@angular/common/http";

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