import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { catchError, map, Observable, of } from 'rxjs';

import { environment } from '../../../environments/environment';

import { SUCCESS_CODE } from '../constants/status-code-constants';

import { ToastHandlingService } from './toast-handling.service';

import type { IndustryModel } from '../models/entities/industry.model';
import type { BaseResponseModel } from '../models/api/base-response.model';

@Injectable({
  providedIn: 'root'
})
export class IndustryService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);
  
  private BASE_API_URL = environment.apiBaseUrl;
  private INDUSTRY_API_URL = `${this.BASE_API_URL}/industries`;
  
  private industriesSignal = signal<IndustryModel[]>([]);
  industries = this.industriesSignal.asReadonly();

  getAllIndustries(): Observable<void> {
    return this.httpClient.get<BaseResponseModel<IndustryModel[]>>(this.INDUSTRY_API_URL)
      .pipe(
        map(res => {
          if (res.statusCode === SUCCESS_CODE) {
            this.industriesSignal.set(res.data || []);
          } else {
            this.industriesSignal.set([]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.industriesSignal.set([]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        })
      );
  }
}
