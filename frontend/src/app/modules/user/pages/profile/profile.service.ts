import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { catchError, map, Observable, of } from 'rxjs'; 

import { environment } from '../../../../../environments/environment';

import { SUCCESS_CODE } from '../../../../shared/constants/status-code-constants';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type { UserModel } from '../../../../shared/models/entities/user.model';
import { SUBSCRIPTIONS } from '../../../../shared/constants/subscription-constant';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private httpClient = inject(HttpClient);
  
  // ? All API base url
  private BASE_API_URL = environment.apiAuthBaseUrl;
  private PROFILE_API_URL = `${this.BASE_API_URL}/users/profile`;

  private userSignal = signal<UserModel | null>(null);
  private subscriptionSignal = signal<string>(SUBSCRIPTIONS[0]);
  user = this.userSignal.asReadonly();
  subscription = this.subscriptionSignal.asReadonly();

  // ? Compute States;
  

  getUserProfile(): Observable<void> {
    return this.httpClient.get<BaseResponseModel<UserModel>>(this.PROFILE_API_URL)
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            this.userSignal.set(res.data);
            this.subscriptionSignal.set(SUBSCRIPTIONS[res.data.subscriptionPlan])
          } else {
            this.userSignal.set(null);
          }
        }),
        catchError(() => {
          return of(undefined);
        })
      );
  }
}
