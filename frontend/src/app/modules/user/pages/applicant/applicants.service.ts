import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { Observable } from 'rxjs';
import { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import { ApplicantResponseModel } from '../../../../shared/models/api/response/applicant-response';
import { ApplicantsRequestModel } from '../../../../shared/models/api/request/applicants-request.model';
import { createHttpParams } from '../../../../shared/utils/request-utils';

@Injectable({
  providedIn: 'root'
})
export class ApplicantsService {

  private httpClient = inject(HttpClient);

  private BASE_API_URL = environment.apiBaseUrl;
  private APPLICANTS_API_URL = `${this.BASE_API_URL}/applicants`;

  
  // getAllApplicantsByJobId(req: ApplicantModel): Observable<BaseResponseModel<ApplicantResponseModel>> {
  //   const reqParams = createHttpParams(req);
  //   return this.httpClient.get<BaseResponseModel<ApplicantResponseModel>>(this.APPLICANTS_API_URL, { params: reqParams });
  // }
  getAllApplicantsByJobId(applicantId: string, req: ApplicantsRequestModel): Observable<BaseResponseModel<ApplicantResponseModel>> {
    const reqParams = createHttpParams(req);
    return this.httpClient.get<BaseResponseModel<ApplicantResponseModel>>(`${this.APPLICANTS_API_URL}/${applicantId}`, {params: reqParams});
  }
}
