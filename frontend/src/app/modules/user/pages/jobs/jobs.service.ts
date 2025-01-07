import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, catchError, finalize, map, of } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { createFilterOptions } from '../../../../shared/utils/util-functions';
import { createHttpParams } from '../../../../shared/utils/request-utils';

import { SUCCESS_CODE } from '../../../../shared/constants/status-code-constants';

import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type { IndustryModel } from '../../../../shared/models/entities/industry.model';
import type { CategoryModel } from '../../../../shared/models/entities/category.model';
import type { CityModel, WardModel } from '../../../../shared/models/entities/location.model';
import type { JobListRequestModel } from '../../../../shared/models/api/request/job-list-request.model';
import type { JobDetailResponseModel, JobsResponseModel } from '../../../../shared/models/api/response/jobs-response.model';
import type { JobPostModel } from '../../../../shared/models/entities/job.model';

@Injectable({
  providedIn: 'root'
})
export class JobsService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);
  private multiLanguageService = inject(MultiLanguageService);

  // ? All API base url
  private BASE_API_URL = environment.apiBaseUrl;
  private JOBS_API_URL = `${this.BASE_API_URL}/jobposts`;
  private INDUSTRY_API_URL = `${this.BASE_API_URL}/industries`;
  private CATEGORY_API_URL = `${this.BASE_API_URL}/categories`;
  private CITIES_API_URL = `${this.BASE_API_URL}/location/cities`;

  // ? Signals for state management
  private jobsSignal = signal<JobPostModel[]>([]);
  private industriesSignal = signal<IndustryModel[]>([]);
  private categoriesSignal = signal<CategoryModel[]>([]);
  private citiesSignal = signal<CityModel[]>([]);
  private wardsSignal = signal<WardModel[]>([]);
  private totalJobsSignal = signal(0);
  private loadingSignal = signal(false);

  // ? Signals for expose
  jobs = this.jobsSignal.asReadonly();
  industries = this.industriesSignal.asReadonly();
  categories = this.categoriesSignal.asReadonly();
  cities = this.citiesSignal.asReadonly();
  wards = this.wardsSignal.asReadonly();
  totalJobs = this.totalJobsSignal.asReadonly();
  isLoading = this.loadingSignal.asReadonly();

  getAllJobs(req: JobListRequestModel): Observable<void> {
    this.loadingSignal.set(true);
    
    return this.httpClient.get<BaseResponseModel<JobsResponseModel>>(this.JOBS_API_URL, { 
      params: createHttpParams(req) 
    }).pipe(
      map(res => {
        if (res.statusCode === SUCCESS_CODE && res.data) {
          const { data, count } = res.data;
          this.jobsSignal.set(data);
          this.totalJobsSignal.set(count);
        } else {
          this.resetJobs();
        }
      }),
      catchError(() => {
        this.resetJobs();
        this.toastHandlingService.handleError('jobs.notify.fetch-jobs-failed');
        return of(void 0);
      }),
      finalize(() => this.loadingSignal.set(false))
    );
  }

  getAllIndustries(): Observable<void> {
    const defaultOptions = createFilterOptions(this.multiLanguageService).industries;
    return this.httpClient.get<BaseResponseModel<IndustryModel[]>>(this.INDUSTRY_API_URL)
      .pipe(
        map(res => {
          if (res.statusCode === SUCCESS_CODE) {
            this.industriesSignal.set([defaultOptions, ...(res.data || [])]);
          } else {
            this.industriesSignal.set([defaultOptions, ...[]]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.industriesSignal.set([defaultOptions, ...[]]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        })
      );
  }

  getAllCategories(): Observable<void> {
    const defaultOptions = createFilterOptions(this.multiLanguageService).categories;
    return this.httpClient.get<BaseResponseModel<CategoryModel[]>>(this.CATEGORY_API_URL)
      .pipe(
        map(res => {
          if (res.statusCode === SUCCESS_CODE) {
            this.categoriesSignal.set([defaultOptions, ...(res.data || [])]);
          } else {
            this.categoriesSignal.set([defaultOptions, ...[]]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.categoriesSignal.set([defaultOptions, ...[]]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        })
      );
  }

  getAllCities(): Observable<void> {
    const defaultOptions = createFilterOptions(this.multiLanguageService).cities;
    return this.httpClient.get<BaseResponseModel<CityModel[]>>(this.CITIES_API_URL)
      .pipe(
        map(res => {
          if (res.statusCode === SUCCESS_CODE) {
            this.citiesSignal.set([defaultOptions, ...(res.data || [])]);
          } else {
            this.citiesSignal.set([defaultOptions, ...[]]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.citiesSignal.set([defaultOptions, ...[]]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        })
      );
  }

  getWardByCityId(cityId: string): Observable<void> {
    const defaultOptions = createFilterOptions(this.multiLanguageService).wards;
    return this.httpClient.get<BaseResponseModel<WardModel[]>>(`${this.CITIES_API_URL}/${cityId}/wards`)
      .pipe(
        map(res => {
          if (res.statusCode === SUCCESS_CODE) {
            this.wardsSignal.set([defaultOptions, ...(res.data || [])]);
          } else {
            this.wardsSignal.set([defaultOptions, ...[]]);
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.wardsSignal.set([defaultOptions, ...[]]);
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        })
      );
  }

  getJobDetailById(jobId: string): Observable<JobDetailResponseModel | null> {
    return this.httpClient.get<BaseResponseModel<JobDetailResponseModel>>(`${this.JOBS_API_URL}/${jobId}`)
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            return res.data;
          }
          return null;
        }),
        catchError(() => of(null))
      );
  }

  private resetJobs(): void {
    this.jobsSignal.set([]);
    this.totalJobsSignal.set(0);
  }
}
