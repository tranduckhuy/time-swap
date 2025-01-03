import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { TOKEN_KEY, REFRESH_TOKEN_KEY, EXPIRES_IN_KEY  } from '../../shared/constants/auth-constants';

import { createHttpParams } from '../../shared/utils/request-utils';

import type { BaseResponseModel } from '../../shared/models/api/base-response.model';
import type { LoginRequestModel, RefreshRequestModel } from '../../shared/models/api/request/login-request.model';
import type { LoginResponseModel } from '../../shared/models/api/response/login-response.model';
import type { RegisterRequestModel } from '../../shared/models/api/request/register-request.model';
import type { ConfirmRequestModel, ReConfirmRequestModel } from '../../shared/models/api/request/confirm-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpClient = inject(HttpClient);

  BASE_API_URL = environment.apiBaseUrl;
  LOGIN_API_URL = `${this.BASE_API_URL}/auth/login`;
  REGISTER_API_URL = `${this.BASE_API_URL}/auth/register`;
  REFRESH_API_URL = `${this.BASE_API_URL}/auth/refresh-token`;
  CONFIRM_API_URL = `${this.BASE_API_URL}/auth/confirm-email`;
  RE_CONFIRM_API_URL = `${this.BASE_API_URL}/auth/resend-confirmation-email`;

  signin(loginReq: LoginRequestModel): Observable<BaseResponseModel<LoginResponseModel>> {
    return this.sendPostRequest<LoginRequestModel, BaseResponseModel<LoginResponseModel>>(
      this.LOGIN_API_URL,
      loginReq
    );
  }
  
  register(registerReq: RegisterRequestModel): Observable<BaseResponseModel> {
    return this.sendPostRequest<RegisterRequestModel, BaseResponseModel>(
      this.REGISTER_API_URL,
      registerReq
    );
  }

  refreshToken(refreshReq: RefreshRequestModel): Observable<BaseResponseModel<LoginResponseModel>> {
    return this.sendPostRequest<RefreshRequestModel, BaseResponseModel<LoginResponseModel>>(
      this.REFRESH_API_URL, 
      refreshReq
    );
  }

  resendConfirmEmail(reConfirmReq: ReConfirmRequestModel): Observable<BaseResponseModel> {
    return this.sendPostRequest<ReConfirmRequestModel, BaseResponseModel>(
      this.RE_CONFIRM_API_URL, 
      reConfirmReq
    );
  }

  confirmEmail(confirmReq: ConfirmRequestModel): Observable<BaseResponseModel> {
    const reqParams = createHttpParams(confirmReq);
    return this.httpClient.get<BaseResponseModel>(this.CONFIRM_API_URL, { params: reqParams });
  }

  isLoggedIn() {
    return !!this.getToken();
  }

  isTokenExpired() {
    const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds so that divide for 1000
    const expirationTime = this.getExpirationTime();

    return expirationTime && parseInt(expirationTime, 10) < currentTime;
  }

  saveLocalData(token: string, refreshToken: string, expiresIn: string) {
    const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds so that divide for 1000
    const expirationTime = currentTime + parseInt(expiresIn, 10);

    localStorage.setItem(TOKEN_KEY, token);  
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);  
    localStorage.setItem(EXPIRES_IN_KEY, expirationTime.toString());  
  }

  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  }

  getRefreshToken() {
    return localStorage.getItem(TOKEN_KEY);
  }

  getExpirationTime() {
    return localStorage.getItem(EXPIRES_IN_KEY);
  }

  deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
  }

  private sendPostRequest<T, R>(url: string, body: T): Observable<R> {
    return this.httpClient.post<R>(
      url,
      JSON.stringify(body),
      {
        headers: {
          'Content-Type': 'application/json',
        },
      }
    );
  }
}
