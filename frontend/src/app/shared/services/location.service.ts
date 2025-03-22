import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';

import { SUCCESS_CODE } from '../constants/status-code-constants';

import { environment } from '../../../environments/environment';

import { ToastHandlingService } from './toast-handling.service';

import type { BaseResponseModel } from '../models/api/base-response.model';
import type { CityModel, WardModel } from '../models/entities/location.model';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);

  // ? All API base url
  private BASE_API_URL = environment.apiBaseUrl;
  private CITIES_API_URL = `${this.BASE_API_URL}/location/cities`;

  // ? Signals for state management
  private citiesSignal = signal<CityModel[]>([]);
  private wardsSignal = signal<WardModel[]>([]);

  // ? Signals for expose
  cities = this.citiesSignal.asReadonly();
  wards = this.wardsSignal.asReadonly();

  getAllCities(): Observable<void> {
    return this.httpClient
      .get<BaseResponseModel<CityModel[]>>(this.CITIES_API_URL)
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE) {
            this.citiesSignal.set(res.data || []);
          } else {
            this.citiesSignal.set([]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.citiesSignal.set([]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        }),
      );
  }

  getWardByCityId(cityId: string): Observable<void> {
    return this.httpClient
      .get<
        BaseResponseModel<WardModel[]>
      >(`${this.CITIES_API_URL}/${cityId}/wards`)
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE) {
            this.wardsSignal.set(res.data || []);
          } else {
            this.wardsSignal.set([]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.wardsSignal.set([]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        }),
      );
  }

  clearWards() {
    this.wardsSignal.set([]);
  }
}
