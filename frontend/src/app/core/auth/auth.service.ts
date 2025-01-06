import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

import { TOKEN_KEY, REFRESH_TOKEN_KEY, EXPIRES_IN_KEY } from '../../shared/constants/auth-constants';
import { SUCCESS_CODE } from '../../shared/constants/status-code-constants';

import { createHttpParams } from '../../shared/utils/request-utils';

import type { BaseResponseModel } from '../../shared/models/api/base-response.model';
import type { LoginRequestModel, RefreshRequestModel } from '../../shared/models/api/request/login-request.model';
import type { LoginResponseModel } from '../../shared/models/api/response/login-response.model';
import type { RegisterRequestModel } from '../../shared/models/api/request/register-request.model';
import type { ConfirmRequestModel, ReConfirmRequestModel } from '../../shared/models/api/request/confirm-request.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpClient = inject(HttpClient);

  private BASE_API_URL = environment.apiAuthBaseUrl;
  private LOGIN_API_URL = `${this.BASE_API_URL}/auth/login`;
  private REGISTER_API_URL = `${this.BASE_API_URL}/auth/register`;
  private LOGOUT_API_URL = `${this.BASE_API_URL}/auth/logout`;
  private REFRESH_API_URL = `${this.BASE_API_URL}/auth/refresh-token`;
  private CONFIRM_API_URL = `${this.BASE_API_URL}/auth/confirm-email`;
  private RE_CONFIRM_API_URL = `${this.BASE_API_URL}/auth/resend-confirmation-email`;

  private loginState = signal<boolean>(this.checkLoginState());
  readonly isLoggedIn = computed(() => this.loginState());

  signin(loginReq: LoginRequestModel): Observable<BaseResponseModel<LoginResponseModel>> {
    return this.sendPostRequest<LoginRequestModel, BaseResponseModel<LoginResponseModel>>(
      this.LOGIN_API_URL,
      loginReq
    );
  }

  register(registerReq: RegisterRequestModel): Observable<BaseResponseModel> {
    return this.sendPostRequest<RegisterRequestModel, BaseResponseModel>(this.REGISTER_API_URL, registerReq);
  }

  logout(): Observable<BaseResponseModel> {
    return this.httpClient.delete<BaseResponseModel>(this.LOGOUT_API_URL).pipe(
      tap((res) => {
        if (res.statusCode === SUCCESS_CODE) {
          this.deleteToken();
          this.updateLoginState();
        }
      })
    );
  }

  refreshTokenApi(refreshReq: RefreshRequestModel): Observable<BaseResponseModel<LoginResponseModel>> {
    return this.sendPostRequest<RefreshRequestModel, BaseResponseModel<LoginResponseModel>>(
      this.REFRESH_API_URL,
      refreshReq
    );
  }

  resendConfirmEmail(reConfirmReq: ReConfirmRequestModel): Observable<BaseResponseModel> {
    return this.sendPostRequest<ReConfirmRequestModel, BaseResponseModel>(this.RE_CONFIRM_API_URL, reConfirmReq);
  }

  confirmEmail(confirmReq: ConfirmRequestModel): Observable<BaseResponseModel> {
    const reqParams = createHttpParams(confirmReq);
    return this.httpClient.get<BaseResponseModel>(this.CONFIRM_API_URL, { params: reqParams });
  }

  saveLocalData(token: string, refreshToken: string, expiresIn: string) {
    const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds
    const expirationTime = currentTime + parseInt(expiresIn, 10);

    localStorage.setItem(TOKEN_KEY, token);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
    localStorage.setItem(EXPIRES_IN_KEY, expirationTime.toString());

    this.updateLoginState();
  }

  deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(EXPIRES_IN_KEY);
  }

  get accessToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  get refreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  get isTokenExpired(): boolean {
    const currentTime = Math.floor(Date.now() / 1000);
    const expirationTime = localStorage.getItem(EXPIRES_IN_KEY);

    return expirationTime ? parseInt(expirationTime, 10) < currentTime : true;
  }

  private checkLoginState(): boolean {
    const token = this.accessToken;
    const refreshToken = this.refreshToken;

    return !!token && !!refreshToken;
  }

  private updateLoginState() {
    this.loginState.set(this.checkLoginState());
  }

  private sendPostRequest<T, R>(url: string, body: T): Observable<R> {
    return this.httpClient.post<R>(url, JSON.stringify(body), {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }
}
