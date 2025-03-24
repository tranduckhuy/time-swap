import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { catchError, map, Observable, of } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import {
  SUCCESS_CODE,
  USER_APPLIED_TO_OWN_JOB_POST,
  NOT_UPDATE_PROFILE_YET,
} from '../../../../shared/constants/status-code-constants';

import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type { UserModel } from '../../../../shared/models/entities/user.model';
import type { ApplicantResponseModel } from '../../../../shared/models/api/response/applicant-response.model';
import type { ApplicantsRequestModel } from '../../../../shared/models/api/request/applicants-request.model';

@Injectable({
  providedIn: 'root',
})
export class ApplicantsService {
  private readonly httpClient = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly toastHandlingService = inject(ToastHandlingService);

  private readonly BASE_API_URL = environment.apiBaseUrl;
  private readonly BASE_AUTH_API_URL = environment.apiAuthBaseUrl;
  private readonly APPLICANTS_API_URL = `${this.BASE_API_URL}/applicants`;
  private readonly USERS_API_URL = `${this.BASE_AUTH_API_URL}/users`;

  getAllApplicantsByJobId(
    applicantId: string,
    req: ApplicantsRequestModel,
  ): Observable<BaseResponseModel<ApplicantResponseModel>> {
    const reqParams = createHttpParams(req);
    return this.httpClient.get<BaseResponseModel<ApplicantResponseModel>>(
      `${this.APPLICANTS_API_URL}/${applicantId}`,
      { params: reqParams },
    );
  }

  getApplicantDetailById(applicantId: string): Observable<UserModel | null> {
    return this.httpClient
      .get<
        BaseResponseModel<UserModel | null>
      >(`${this.USERS_API_URL}/${applicantId}`)
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            return res.data;
          }
          return null;
        }),
        catchError(() => {
          this.toastHandlingService.handleCommonError(); // Handle common error
          return of(null); // Return null if error
        }),
      );
  }

  applyJobById(jobId: string): Observable<void> {
    const currentUrl = this.router.url;
    const req = {
      jobPostId: jobId,
    };
    return this.httpClient
      .post<BaseResponseModel<void>>(
        this.APPLICANTS_API_URL,
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
                'job-detail.notify.success',
              );
              this.router
                .navigateByUrl('/dummy', { skipLocationChange: true })
                .then(() => this.router.navigateByUrl(currentUrl));
              break;
            case USER_APPLIED_TO_OWN_JOB_POST:
              this.toastHandlingService.handleWarning(
                'job-detail.notify.apply-own-job',
              );
              break;
            case NOT_UPDATE_PROFILE_YET:
              this.toastHandlingService.handleWarning(
                'jobs.notify.not-update-profile',
              );
              break;
            default:
              this.toastHandlingService.handleError('job-detail.notify.failed');
          }
        }),
        catchError(() => {
          this.toastHandlingService.handleCommonError();
          return of(undefined);
        }),
      );
  }
}
