import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

import { Observable, catchError, map, of, shareReplay, tap } from 'rxjs';

import { environment } from '../../../environments/environment';

import { createHttpParams } from '../../shared/utils/request-utils';

import { TOKEN_KEY, REFRESH_TOKEN_KEY, EXPIRES_IN_KEY } from '../../shared/constants/auth-constants';
import { 
  EMAIL_EXIST_CODE, 
  INVALID_CREDENTIAL_CODE, 
  NOT_CONFIRM_CODE, 
  REGISTER_CONFIRM_SUCCESS_CODE, 
  SUCCESS_CODE, 
  TOKEN_EXPIRED_CODE, 
  USER_NOT_EXIST_CODE 
} from '../../shared/constants/status-code-constants';

import { ToastHandlingService } from '../../shared/services/toast-handling.service';

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
  private router = inject(Router);
  private toastHandlingService = inject(ToastHandlingService);

  // ? All API base url
  private BASE_API_URL = environment.apiAuthBaseUrl;
  private LOGIN_API_URL = `${this.BASE_API_URL}/auth/login`;
  private REGISTER_API_URL = `${this.BASE_API_URL}/auth/register`;
  private LOGOUT_API_URL = `${this.BASE_API_URL}/auth/logout`;
  private REFRESH_API_URL = `${this.BASE_API_URL}/auth/refresh-token`;
  private CONFIRM_API_URL = `${this.BASE_API_URL}/auth/confirm-email`;
  private RE_CONFIRM_API_URL = `${this.BASE_API_URL}/auth/resend-confirmation-email`;

  // ? Signals for state management
  private loginState = signal<boolean>(this.checkLoginState());
  readonly isLoggedIn = computed(() => this.loginState());

  signin(loginReq: LoginRequestModel): Observable<void> {
    return this.sendPostRequest<LoginRequestModel, BaseResponseModel<LoginResponseModel>>(
      this.LOGIN_API_URL,
      loginReq
    ).pipe(
      map((res) => {
        if (res.data) {
          const { accessToken, refreshToken, expiresIn } = res.data;
          this.saveLocalData(accessToken, refreshToken, expiresIn);
          this.toastHandlingService.handleSuccess('auth.login.success');
          this.router.navigateByUrl('/home', {
            replaceUrl: true
          });
        } else {
          this.toastHandlingService.handleCommonError();
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.error.statusCode === NOT_CONFIRM_CODE) {
          this.toastHandlingService.handleWarning('auth.login.not-confirm');
        } else if (error.error.statusCode === USER_NOT_EXIST_CODE) {
          this.toastHandlingService.handleWarning('auth.login.user-not-exist');
        } else if (error.error.statusCode === INVALID_CREDENTIAL_CODE) {
          this.toastHandlingService.handleInfo('auth.login.invalid-credentials');
        } else {
          this.toastHandlingService.handleCommonError();
        }
        return of(undefined);
      })
    );
  }

  register(registerReq: RegisterRequestModel): Observable<void> {
    return this.sendPostRequest<RegisterRequestModel, BaseResponseModel>(this.REGISTER_API_URL, registerReq)
      .pipe(
        map((res) => {
          if (res.statusCode === REGISTER_CONFIRM_SUCCESS_CODE) {
            this.toastHandlingService.handleSuccess('auth.register.success');
          } else {
            this.toastHandlingService.handleError('auth.register.failure');
          }
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.error.statusCode === EMAIL_EXIST_CODE) {
            this.toastHandlingService.handleWarning('auth.register.email-exist');
          } else {
            this.toastHandlingService.handleCommonError();
          }
          return of(undefined);
        })
      );
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
    ).pipe(
      tap(response => {
        if (!response.data) {
          throw new Error(response.message || 'Invalid refresh token response');
        }
        const { accessToken, refreshToken, expiresIn } = response.data;
        this.saveLocalData(accessToken, refreshToken, expiresIn);
      }),
      shareReplay(1)
    );
  }

  resendConfirmEmail(reConfirmReq: ReConfirmRequestModel): Observable<void> {
    return this.sendPostRequest<ReConfirmRequestModel, BaseResponseModel>(this.RE_CONFIRM_API_URL, reConfirmReq)
      .pipe(
        map((res) => {
          if (res.statusCode === REGISTER_CONFIRM_SUCCESS_CODE) {
            this.toastHandlingService.handleSuccess('auth.login.re-confirm-success');
          } else {
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError(() => {
          this.toastHandlingService.handleCommonError();
          return of(undefined);
        })
      );
  }

  confirmEmail(confirmReq: ConfirmRequestModel, email: string, clientUrl: string): Observable<void> {
    const reqParams = createHttpParams(confirmReq);
    return this.httpClient.get<BaseResponseModel>(this.CONFIRM_API_URL, { params: reqParams })
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE) {
            this.toastHandlingService.handleSuccess('auth.login.confirm-success');
          } else {
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.error.statusCode === TOKEN_EXPIRED_CODE) {
            const resendReq: ReConfirmRequestModel = {
              email,
              clientUrl
            }
            this.toastHandlingService.handleWarning('auth.login.token-expired');
            this.resendConfirmEmail(resendReq);
          } else {
            this.toastHandlingService.handleCommonError();
          }
          return of(undefined);
        })
      );
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
