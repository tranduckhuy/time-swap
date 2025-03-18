import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, catchError, finalize, map, of } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import {
  SUCCESS_CODE,
  DUE_DATE_CURRENT_FAILED,
  FEE_GREATER_THAN_FIFTY,
  DUE_DATE_START_FAILED,
  USER_NOT_ENOUGH_BALANCE,
} from '../../../../shared/constants/status-code-constants';

import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type { JobListRequestModel } from '../../../../shared/models/api/request/job-list-request.model';
import type {
  JobDetailResponseModel,
  JobsResponseModel,
} from '../../../../shared/models/api/response/jobs-response.model';
import type { JobPostModel } from '../../../../shared/models/entities/job.model';
import type { PostJobRequestModel } from '../../../../shared/models/api/request/post-job-request.model';

@Injectable({
  providedIn: 'root',
})
export class JobsService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);
  private multiLanguageService = inject(MultiLanguageService);

  // ? All API base url
  private BASE_API_URL = environment.apiBaseUrl;
  private JOBS_API_URL = `${this.BASE_API_URL}/jobposts`;

  // ? Signals for state management
  private jobsSignal = signal<JobPostModel[]>([]);
  private totalJobsSignal = signal(0);
  private isLoadingSignal = signal(false);

  // ? Signals for expose
  jobs = this.jobsSignal.asReadonly();
  totalJobs = this.totalJobsSignal.asReadonly();
  isLoading = this.isLoadingSignal.asReadonly();

  getAllJobs(req: JobListRequestModel): Observable<void> {
    this.isLoadingSignal.set(true);

    return this.httpClient
      .get<BaseResponseModel<JobsResponseModel>>(this.JOBS_API_URL, {
        params: createHttpParams(req),
      })
      .pipe(
        map((res) => {
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
          this.toastHandlingService.handleError(
            'jobs.notify.fetch-jobs-failed',
          );
          return of(void 0);
        }),
        finalize(() => this.isLoadingSignal.set(false)),
      );
  }

  createJob(req: PostJobRequestModel): Observable<void> {
    this.isLoadingSignal.set(true);

    return this.httpClient
      .post<BaseResponseModel>(this.JOBS_API_URL, JSON.stringify(req), {
        headers: {
          'Content-Type': 'application/json',
        },
      })
      .pipe(
        map((res) => {
          switch (res.statusCode) {
            case SUCCESS_CODE:
              this.toastHandlingService.handleSuccess(
                'jobs.notify.create-job.success',
              );
              break;
            case USER_NOT_ENOUGH_BALANCE:
              this.toastHandlingService.handleError(
                'jobs.notify.create-job.user-not-enough-balance',
              );
              break;
            case DUE_DATE_START_FAILED:
              this.toastHandlingService.handleError(
                'jobs.notify.create-job.due-date-start-failed',
              );
              break;
            case DUE_DATE_CURRENT_FAILED:
              this.toastHandlingService.handleError(
                'jobs.notify.create-job.due-date-current-failed',
              );
              break;
            case FEE_GREATER_THAN_FIFTY:
              this.toastHandlingService.handleError(
                'jobs.notify.create-job.fee-greater-than-fifty-thousand',
              );
              break;
            default:
              this.toastHandlingService.handleError(
                'jobs.notify.create-job.failed',
              );
          }
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            return of(void 0);
          }
          this.toastHandlingService.handleError(
            'jobs.notify.create-job.failed',
          );
          return of(void 0);
        }),
        finalize(() => this.isLoadingSignal.set(false)),
      );
  }

  getJobDetailById(jobId: string): Observable<JobDetailResponseModel | null> {
    return this.httpClient
      .get<
        BaseResponseModel<JobDetailResponseModel>
      >(`${this.JOBS_API_URL}/${jobId}`)
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            res.data.responsibilitiesList = res.data.responsibilities
              ? res.data.responsibilities
                  .split(/,\s*/)
                  .map((responsibility) => {
                    const trimmed = responsibility.trim().replace(/\.$/, '');
                    const capitalized =
                      trimmed.charAt(0).toUpperCase() + trimmed.slice(1);
                    return `${capitalized}.`;
                  })
              : [
                  this.multiLanguageService.getTranslatedLang(
                    'job-detail.no-responsibilities',
                  ),
                ];
            return res.data;
          }
          return null;
        }),
        catchError(() => of(null)),
      );
  }

  private resetJobs(): void {
    this.jobsSignal.set([]);
    this.totalJobsSignal.set(0);
  }
}
