import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { catchError, map, Observable, of } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import {
  SUCCESS_CODE,
  USER_APPLIED_TO_OWN_JOB_POST,
} from '../../../../shared/constants/status-code-constants';

import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type { ApplicantResponseModel } from '../../../../shared/models/api/response/applicant-response.model';
import type { ApplicantsRequestModel } from '../../../../shared/models/api/request/applicants-request.model';

@Injectable({
  providedIn: 'root',
})
export class ApplicantsService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);

  private BASE_API_URL = environment.apiBaseUrl;
  private APPLICANTS_API_URL = `${this.BASE_API_URL}/applicants`;

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

  applyJobById(jobId: string): Observable<void> {
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
          if (res.statusCode === SUCCESS_CODE) {
            this.toastHandlingService.handleSuccess(
              'job-detail.notify.success',
            );
          } else if (res.statusCode === USER_APPLIED_TO_OWN_JOB_POST) {
            this.toastHandlingService.handleWarning(
              'job-detail.notify.apply-own-job',
            );
          } else {
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
