import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { catchError, map, Observable, of } from 'rxjs';

import { environment } from '../../../environments/environment';

import { SUCCESS_CODE } from '../constants/status-code-constants';

import { ToastHandlingService } from './toast-handling.service';

import type { BaseResponseModel } from '../models/api/base-response.model';
import type { CategoryModel } from '../models/entities/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);

  private BASE_API_URL = environment.apiBaseUrl;
  private INDUSTRY_API_URL = `${this.BASE_API_URL}/industries`;
  private CATEGORY_API_URL = `${this.BASE_API_URL}/categories`;

  private categoriesSignal = signal<CategoryModel[]>([]);
  categories = this.categoriesSignal.asReadonly();

  getAllCategories(): Observable<void> {
    return this.httpClient.get<BaseResponseModel<CategoryModel[]>>(this.CATEGORY_API_URL)
      .pipe(
        map(res => {
          if (res.statusCode === SUCCESS_CODE) {
            this.categoriesSignal.set(res.data || []);
          } else {
            this.categoriesSignal.set([]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.categoriesSignal.set([]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        })
      );
  }

  getCategoriesByIndustryId(industryId: number): Observable<void> {
      return this.httpClient.get<BaseResponseModel<CategoryModel[]>>(`${this.INDUSTRY_API_URL}/${industryId}/categories`)
        .pipe(
          map(res => {
            if (res.statusCode === SUCCESS_CODE) {
              this.categoriesSignal.set(res.data || []);
            } else {
              this.categoriesSignal.set([]);
              this.toastHandlingService.handleCommonError();
            }
          }),
          catchError(() => {
            this.categoriesSignal.set([]);
            this.toastHandlingService.handleCommonError();
            return of(void 0);
          })
        );
    }
}
