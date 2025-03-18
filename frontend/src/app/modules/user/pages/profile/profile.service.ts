import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, finalize, Observable, of, switchMap } from 'rxjs';

import { environment } from '../../../../../environments/environment';
import { SUCCESS_CODE } from '../../../../shared/constants/status-code-constants';
import { SUBSCRIPTIONS } from '../../../../shared/constants/subscription-constant';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type { JobPostModel } from '../../../../shared/models/entities/job.model';
import type {
  CityModel,
  WardModel,
} from '../../../../shared/models/entities/location.model';
import type {
  UserModel,
  UserUpdateModel,
} from '../../../../shared/models/entities/user.model';
import { IndustryModel } from '../../../../shared/models/entities/industry.model';
import { CategoryModel } from '../../../../shared/models/entities/category.model';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private readonly httpClient = inject(HttpClient);

  // ? API Base URLs
  private readonly BASE_AUTH_API_URL = environment.apiAuthBaseUrl;
  private readonly BASE_API_URL = environment.apiBaseUrl;
  private readonly PROFILE_API_URL = `${this.BASE_AUTH_API_URL}/users/profile`;
  private readonly JOB_POSTS_API_URL = `${this.BASE_API_URL}/jobposts/user`;

  // ? State Management Signals
  private readonly userSignal = signal<UserModel | null>(null);
  private readonly jobsSignal = signal<JobPostModel[]>([]);
  private readonly subscriptionSignal = signal<string>(SUBSCRIPTIONS[0]);
  private readonly loadingSignal = signal(false);

  // ? Readonly Exposed Signals
  user = this.userSignal.asReadonly();
  jobs = this.jobsSignal.asReadonly();
  subscription = this.subscriptionSignal.asReadonly();
  isLoading = this.loadingSignal.asReadonly();

  getUserProfile(): Observable<void> {
    this.loadingSignal.set(true);
    return this.httpClient
      .get<BaseResponseModel<UserModel>>(this.PROFILE_API_URL)
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            this.userSignal.set(res.data);
            this.subscriptionSignal.set(
              SUBSCRIPTIONS[res.data.subscriptionPlan],
            );
          } else {
            this.userSignal.set(null);
          }
        }),
        catchError(() => of(undefined)),
        finalize(() => this.loadingSignal.set(false)),
      );
  }

  updateUserProfile(
    updatedProfile: Partial<UserUpdateModel>,
    industries: IndustryModel[],
    categories: CategoryModel[],
    cities: CityModel[],
    wards: WardModel[],
    user: UserModel,
  ): Observable<boolean> {
    this.loadingSignal.set(true);

    // Keep old industryId if just categoryId change
    updatedProfile.majorIndustryId ??= user.majorIndustry.id;
    // Keep old cityId if just wardId change
    updatedProfile.cityId ??= user.city.id;

    const filteredProfile = this.filterEmptyValues(updatedProfile);

    return this.httpClient
      .put<BaseResponseModel<null>>(this.PROFILE_API_URL, filteredProfile)
      .pipe(
        switchMap((res) => {
          if (res.statusCode === SUCCESS_CODE) {
            this.updateUserSignal(
              filteredProfile,
              industries,
              categories,
              cities,
              wards,
              user,
            );
            return this.getUserProfile().pipe(map(() => true));
          }
          return of(false);
        }),
        catchError(() => of(false)),
        finalize(() => this.loadingSignal.set(false)),
      );
  }

  getJobPostsByUserId(isOwner: boolean, userId: string): Observable<void> {
    this.loadingSignal.set(true);
    return this.httpClient
      .get<BaseResponseModel<JobPostModel | JobPostModel[]>>(
        `${this.JOB_POSTS_API_URL}/${userId}`,
        {
          params: createHttpParams({ isOwner }),
        },
      )
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            // Convert single object or null to an array
            const jobPosts = Array.isArray(res.data) ? res.data : [res.data];
            this.jobsSignal.set(jobPosts);
          } else {
            this.jobsSignal.set([]);
          }
        }),
        catchError(() => {
          this.jobsSignal.set([]);
          return of(undefined);
        }),
        finalize(() => this.loadingSignal.set(false)),
      );
  }

  private filterEmptyValues(
    profile: Partial<UserUpdateModel>,
  ): Partial<UserUpdateModel> {
    return Object.fromEntries(
      Object.entries(profile).filter(([, value]) => value !== ''),
    );
  }

  private updateUserSignal(
    updatedProfile: Partial<UserUpdateModel>,
    industries: IndustryModel[],
    categories: CategoryModel[],
    cities: CityModel[],
    wards: WardModel[],
    user: UserModel,
  ): void {
    const transformedData = this.transformData(
      updatedProfile,
      industries,
      categories,
      cities,
      wards,
      user,
    );
    this.userSignal.set({ ...this.userSignal()!, ...transformedData });
  }

  private transformData(
    input: Partial<UserUpdateModel>,
    industries: IndustryModel[],
    categories: CategoryModel[],
    cities: CityModel[],
    wards: WardModel[],
    user: UserModel,
  ): Partial<UserModel> {
    const { cityId, wardId, majorIndustryId, majorCategoryId, ...rest } = input;

    return {
      ...rest,
      city: cities.find((c) => c.id === cityId) ?? user.city,
      ward: wards.find((w) => w.id === wardId) ?? user.ward,
      majorIndustry:
        industries.find((i) => i.id === majorIndustryId) ?? user.majorIndustry,
      majorCategory:
        categories.find((c) => c.id === majorCategoryId) ?? user.majorCategory,
    };
  }
}
