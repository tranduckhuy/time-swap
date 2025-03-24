import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

import { Observable, catchError, finalize, map, of } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import {
  SUCCESS_CODE,
  DUE_DATE_CURRENT_FAILED,
  FEE_GREATER_THAN_FIFTY,
  DUE_DATE_START_FAILED,
  USER_NOT_ENOUGH_BALANCE,
  USER_NOT_APPLIED_TO_JOB_POST,
  JOB_POST_ALREADY_ASSIGNED,
  ASSIGN_TO_OWNER,
  OWNER_JOB_POST_MISMATCH,
  NOT_UPDATE_PROFILE_YET,
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
import type { AssignJobRequestModel } from '../../../../shared/models/api/request/assign-job-request.model';
import { formatCustomCurrency } from '../../../../shared/utils/util-functions';

@Injectable({
  providedIn: 'root',
})
export class JobsService {
  // ? Dependency Inject
  private readonly httpClient = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly toastHandlingService = inject(ToastHandlingService);
  private readonly multiLanguageService = inject(MultiLanguageService);

  // ? All API base url
  private readonly BASE_API_URL = environment.apiBaseUrl;
  private readonly JOBS_API_URL = `${this.BASE_API_URL}/jobposts`;
  private readonly ASSIGN_JOB_API_URL = `${this.BASE_API_URL}/jobposts`;

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
              this.router.navigateByUrl('jobs');
              this.toastHandlingService.handleSuccess(
                'jobs.notify.create-job.success',
              );
              break;
            case USER_NOT_ENOUGH_BALANCE:
              const formattedFee = formatCustomCurrency(
                req.fee,
                this.multiLanguageService.currentLang as 'vi' | 'en',
              );
              this.toastHandlingService.handleWarning(
                'jobs.notify.create-job.user-not-enough-balance',
                {
                  fee: formattedFee,
                },
              );
              break;
            case DUE_DATE_START_FAILED:
              this.toastHandlingService.handleWarning(
                'jobs.notify.create-job.due-date-start-failed',
              );
              break;
            case DUE_DATE_CURRENT_FAILED:
              this.toastHandlingService.handleWarning(
                'jobs.notify.create-job.due-date-current-failed',
              );
              break;
            case FEE_GREATER_THAN_FIFTY:
              this.toastHandlingService.handleWarning(
                'jobs.notify.create-job.fee-greater-than-fifty-thousand',
              );
              break;
            case NOT_UPDATE_PROFILE_YET:
              this.toastHandlingService.handleWarning(
                'jobs.notify.not-update-profile',
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
          this.toastHandlingService.handleCommonError();
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

  assignJob(req: AssignJobRequestModel): Observable<void> {
    const currentUrl = this.router.url;
    return this.httpClient
      .post<BaseResponseModel<void>>(
        `${this.ASSIGN_JOB_API_URL}/${req.jobPostId}/apply`,
        JSON.stringify(req),
        {
          headers: {
            'Content-Type': 'application/json',
          },
        },
      )
      .pipe(
        map((res) => {
          switch (res.statusCode) {
            case SUCCESS_CODE:
              this.toastHandlingService.handleSuccess(
                'jobs.notify.assign-job.success',
              );
              this.router
                .navigateByUrl('/dummy', { skipLocationChange: true })
                .then(() => this.router.navigateByUrl(currentUrl));
              break;
            case USER_NOT_APPLIED_TO_JOB_POST:
              this.toastHandlingService.handleWarning(
                'jobs.notify.assign-job.not-applied',
              );
              break;
            case JOB_POST_ALREADY_ASSIGNED:
              this.toastHandlingService.handleWarning(
                'jobs.notify.assign-job.already-assigned',
              );
              break;
            case ASSIGN_TO_OWNER:
              this.toastHandlingService.handleWarning(
                'jobs.notify.assign-job.assign-to-owner',
              );
              break;
            case OWNER_JOB_POST_MISMATCH:
              this.toastHandlingService.handleWarning(
                'jobs.notify.assign-job.owner-mismatch',
              );
              break;
            default:
              this.toastHandlingService.handleError(
                'jobs.notify.assign-job.failed',
              );
              break;
          }
        }),
        catchError(() => {
          this.toastHandlingService.handleCommonError();
          return of(void 0);
        }),
      );
  }

  private resetJobs(): void {
    this.jobsSignal.set([]);
    this.totalJobsSignal.set(0);
  }
}
