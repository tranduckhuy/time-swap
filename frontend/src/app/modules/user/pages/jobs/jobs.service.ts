import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type { IndustryModel } from '../../../../shared/models/entities/industry.model';
import type { CategoryModel } from '../../../../shared/models/entities/category.model';
import type { CityModel, WardModel } from '../../../../shared/models/entities/location.model';
import type { JobPostModel } from '../../../../shared/models/entities/job.model';
import type { JobListRequestModel } from '../../../../shared/models/api/request/job-list-request.model';
import type { JobsResponseModel } from '../../../../shared/models/api/response/jobs-response.model';

@Injectable({
  providedIn: 'root'
})
export class JobsService {
  private httpClient = inject(HttpClient);

  private BASE_API_URL = environment.apiBaseUrl;
  private JOBS_API_URL = `${this.BASE_API_URL}/jobposts`;
  private INDUSTRY_API_URL = `${this.BASE_API_URL}/industries`;
  private CATEGORY_API_URL = `${this.BASE_API_URL}/categories`;
  private CITIES_API_URL = `${this.BASE_API_URL}/location/cities`;

  getAllJobs(req: JobListRequestModel): Observable<BaseResponseModel<JobsResponseModel>> {
    const reqParams = createHttpParams(req);
    return this.httpClient.get<BaseResponseModel<JobsResponseModel>>(this.JOBS_API_URL, { params: reqParams });
  }

  getAllIndustries(): Observable<BaseResponseModel<IndustryModel[]>> {
    return this.httpClient.get<BaseResponseModel<IndustryModel[]>>(this.INDUSTRY_API_URL);
  }

  getAllCategories(): Observable<BaseResponseModel<CategoryModel[]>> {
    return this.httpClient.get<BaseResponseModel<CategoryModel[]>>(this.CATEGORY_API_URL);
  }

  getAllCities(): Observable<BaseResponseModel<CityModel[]>> {
    return this.httpClient.get<BaseResponseModel<CityModel[]>>(this.CITIES_API_URL);
  }

  getWardByCityId(cityId: string): Observable<BaseResponseModel<WardModel[]>> {
    return this.httpClient.get<BaseResponseModel<WardModel[]>>(`${this.CITIES_API_URL}/${cityId}/wards`);
  }

  getJobDetailById(jobId: string): Observable<BaseResponseModel<JobPostModel>> {
    return this.httpClient.get<BaseResponseModel<JobPostModel>>(`${this.JOBS_API_URL}/${jobId}`);
  }
}
