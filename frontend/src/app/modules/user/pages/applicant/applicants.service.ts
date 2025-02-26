import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { map, Observable } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { SUCCESS_CODE } from '../../../../shared/constants/status-code-constants';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import { ApplicantResponseModel } from '../../../../shared/models/api/response/applicant-response.model';
import { ApplicantsRequestModel } from '../../../../shared/models/api/request/applicants-request.model';
import { UserModel } from '../../../../shared/models/entities/user.model';

@Injectable({
  providedIn: 'root',
})
export class ApplicantsService {
  private httpClient = inject(HttpClient);

  private BASE_API_URL = environment.apiBaseUrl;
  private BASE_AUTH_API_URL = environment.apiAuthBaseUrl;
  private APPLICANTS_API_URL = `${this.BASE_API_URL}/applicants`;
  private USERS_API_URL = `${this.BASE_AUTH_API_URL}/users`;

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
      );
  }
}
