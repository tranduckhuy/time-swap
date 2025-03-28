import {
  Injectable,
  inject,
  signal,
  computed,
  DestroyRef,
  OnDestroy,
} from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

import {
  Observable,
  Subject,
  catchError,
  filter,
  finalize,
  map,
  of,
  switchMap,
  take,
  takeUntil,
  tap,
  throwError,
} from 'rxjs';

import { environment } from '../../../environments/environment';

import { createHttpParams } from '../../shared/utils/request-utils';

import {
  TOKEN_KEY,
  REFRESH_TOKEN_KEY,
  EXPIRES_IN_KEY,
} from '../../shared/constants/auth-constants';
import {
  EMAIL_EXIST_CODE,
  FORGOT_PASSWORD_SUCCESS_CODE,
  INVALID_CREDENTIAL_CODE,
  INVALID_TOKEN,
  NOT_CONFIRM_CODE,
  REGISTER_CONFIRM_SUCCESS_CODE,
  RESET_PASSWORD_SUCCESS_CODE,
  SUCCESS_CODE,
  TOKEN_EXPIRED_CODE,
  USER_ALREADY_CONFIRMED,
  USER_NOT_EXIST_CODE,
} from '../../shared/constants/status-code-constants';

import { ToastHandlingService } from '../../shared/services/toast-handling.service';

import type { BaseResponseModel } from '../../shared/models/api/base-response.model';
import type {
  LoginRequestModel,
  RefreshRequestModel,
} from '../../shared/models/api/request/login-request.model';
import type { LoginResponseModel } from '../../shared/models/api/response/login-response.model';
import type { RegisterRequestModel } from '../../shared/models/api/request/register-request.model';
import type {
  ConfirmRequestModel,
  ReConfirmRequestModel,
} from '../../shared/models/api/request/confirm-request.model';
import type { ForgotPasswordRequestModel } from '../../shared/models/api/request/forgot-password-request.model';
import type { ResetPasswordRequestModel } from '../../shared/models/api/request/reset-password-request.model';
import { toObservable } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class AuthService implements OnDestroy {
  private httpClient = inject(HttpClient);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);
  private toastHandlingService = inject(ToastHandlingService);

  // ? All API base url
  private BASE_API_URL = environment.apiAuthBaseUrl;
  private LOGIN_API_URL = `${this.BASE_API_URL}/auth/login`;
  private REGISTER_API_URL = `${this.BASE_API_URL}/auth/register`;
  private LOGOUT_API_URL = `${this.BASE_API_URL}/auth/logout`;
  private REFRESH_API_URL = `${this.BASE_API_URL}/auth/refresh-token`;
  private CONFIRM_API_URL = `${this.BASE_API_URL}/auth/confirm-email`;
  private RE_CONFIRM_API_URL = `${this.BASE_API_URL}/auth/resend-confirmation-email`;
  private FORGOT_PASSWORD_API_URL = `${this.BASE_API_URL}/auth/forgot-password`;
  private RESET_PASSWORD_API_URL = `${this.BASE_API_URL}/auth/reset-password`;

  // ? Signals for state management
  private loginState = signal<boolean>(this.checkLoginState());
  readonly isLoggedIn = computed(() => this.loginState());

  // ? Refresh statement
  private isRefreshing = signal(false);
  private currentRefreshToken = signal<string | null>(null);
  private destroy$ = new Subject<void>();

  // ? Computed property
  readonly isRefreshingState = computed(() => this.isRefreshing());
  readonly currentToken = computed(() => this.currentRefreshToken());

  // ? Constant for redirect after confirm mail
  private AUTH_CLIENT_URL = environment.authClientUrl;

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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

    return expirationTime
      ? parseInt(expirationTime, 10) < currentTime + 2
      : true;
  }

  get currentToken$(): Observable<string | null> {
    return toObservable(this.currentRefreshToken);
  }

  signin(loginReq: LoginRequestModel): Observable<void> {
    return this.sendPostRequest<
      LoginRequestModel,
      BaseResponseModel<LoginResponseModel>
    >(this.LOGIN_API_URL, loginReq).pipe(
      map((res) => {
        if (res.data) {
          const { accessToken, refreshToken, expiresIn } = res.data;
          this.saveLocalData(accessToken, refreshToken, expiresIn);
          this.toastHandlingService.handleSuccess('auth.login.success');
          this.router.navigateByUrl('/home', {
            replaceUrl: true,
          });
        } else {
          this.toastHandlingService.handleCommonError();
        }
      }),
      catchError((error: HttpErrorResponse) => {
        switch (error.error.statusCode) {
          case NOT_CONFIRM_CODE: {
            const resendReq: ReConfirmRequestModel = {
              email: loginReq.email,
              clientUrl: `${this.AUTH_CLIENT_URL!}/login`,
            };
            this.toastHandlingService.handleWarning('auth.login.not-confirm');
            const subscription = this.resendConfirmEmail(resendReq).subscribe();
            this.destroyRef.onDestroy(() => subscription.unsubscribe());
            break;
          }
          case USER_NOT_EXIST_CODE:
            this.toastHandlingService.handleWarning(
              'auth.login.user-not-exist',
            );
            break;
          case INVALID_CREDENTIAL_CODE:
            this.toastHandlingService.handleInfo(
              'auth.login.invalid-credentials',
            );
            break;
          default:
            this.toastHandlingService.handleCommonError();
        }
        return of(undefined);
      }),
    );
  }

  register(registerReq: RegisterRequestModel): Observable<void> {
    registerReq = {
      ...registerReq,
      clientUrl: `${this.AUTH_CLIENT_URL!}/login`,
    };
    return this.sendPostRequest<RegisterRequestModel, BaseResponseModel>(
      this.REGISTER_API_URL,
      registerReq,
    ).pipe(
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
      }),
    );
  }

  logout(): Observable<BaseResponseModel> {
    return this.httpClient.delete<BaseResponseModel>(this.LOGOUT_API_URL).pipe(
      tap((res) => {
        if (res.statusCode === SUCCESS_CODE) {
          this.deleteToken();
          this.updateLoginState();
        }
      }),
    );
  }

  refreshTokenApi(
    refreshReq: RefreshRequestModel,
  ): Observable<BaseResponseModel<LoginResponseModel>> {
    if (this.isRefreshingState()) {
      return this.currentToken$.pipe(
        filter((token) => !!token),
        take(1),
        switchMap((token) => this.createMockResponse(token ?? '')),
      );
    }

    this.setRefreshingState(true, null);

    return this.sendPostRequest<
      RefreshRequestModel,
      BaseResponseModel<LoginResponseModel>
    >(this.REFRESH_API_URL, refreshReq).pipe(
      tap((response) => this.handleRefreshSuccess(response)),
      catchError((error) => this.handleRefreshError(error)),
      finalize(() => {
        queueMicrotask(() => {
          if (!this.destroy$.closed) {
            this.isRefreshing.set(false);
          }
        });
      }),
      takeUntil(this.destroy$),
    );
  }

  resendConfirmEmail(reConfirmReq: ReConfirmRequestModel): Observable<void> {
    return this.sendPostRequest<ReConfirmRequestModel, BaseResponseModel>(
      this.RE_CONFIRM_API_URL,
      reConfirmReq,
    ).pipe(
      map((res) => {
        if (res.statusCode === REGISTER_CONFIRM_SUCCESS_CODE) {
          this.toastHandlingService.handleSuccess(
            'auth.login.re-confirm-success',
          );
        } else {
          this.toastHandlingService.handleCommonError();
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.error.statusCode === USER_ALREADY_CONFIRMED) {
          this.toastHandlingService.handleWarning(
            'auth.login.user-already-confirmed',
          );
        }
        this.toastHandlingService.handleCommonError();
        return of(undefined);
      }),
    );
  }

  confirmEmail(
    confirmReq: ConfirmRequestModel,
    email: string,
  ): Observable<void> {
    const reqParams = createHttpParams(confirmReq);
    return this.httpClient
      .get<BaseResponseModel>(this.CONFIRM_API_URL, { params: reqParams })
      .pipe(
        map((res) => {
          if (res.statusCode === SUCCESS_CODE) {
            this.toastHandlingService.handleSuccess(
              'auth.login.confirm-success',
            );
          } else {
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.error.statusCode === TOKEN_EXPIRED_CODE) {
            const resendReq: ReConfirmRequestModel = {
              email,
              clientUrl: `${this.AUTH_CLIENT_URL!}/login`,
            };
            this.toastHandlingService.handleWarning('auth.login.token-expired');
            const subscription = this.resendConfirmEmail(resendReq).subscribe();
            this.destroyRef.onDestroy(() => subscription.unsubscribe());
          } else {
            this.toastHandlingService.handleCommonError();
          }
          return of(undefined);
        }),
      );
  }

  forgotPassword(forgotReq: ForgotPasswordRequestModel): Observable<void> {
    forgotReq = {
      ...forgotReq,
      clientUrl: `${this.AUTH_CLIENT_URL!}/reset-password`,
    };
    return this.sendPostRequest<ForgotPasswordRequestModel, BaseResponseModel>(
      this.FORGOT_PASSWORD_API_URL,
      forgotReq,
    ).pipe(
      map((res) => {
        if (res.statusCode === FORGOT_PASSWORD_SUCCESS_CODE) {
          this.toastHandlingService.handleSuccess('auth.forgot.success');
        } else {
          this.toastHandlingService.handleError('auth.forgot.failure');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.error.statusCode === USER_NOT_EXIST_CODE) {
          this.toastHandlingService.handleWarning(
            'auth.forgot.email-not-exist',
          );
        } else {
          this.toastHandlingService.handleCommonError();
        }
        return of(undefined);
      }),
    );
  }

  resetPassword(resetReq: ResetPasswordRequestModel): Observable<void> {
    return this.sendPostRequest<ResetPasswordRequestModel, BaseResponseModel>(
      this.RESET_PASSWORD_API_URL,
      resetReq,
    ).pipe(
      map((res) => {
        if (res.statusCode === RESET_PASSWORD_SUCCESS_CODE) {
          this.toastHandlingService.handleSuccess('auth.reset.success');
          this.router.navigateByUrl('/auth/login', {
            replaceUrl: true,
          });
        } else {
          this.toastHandlingService.handleError('auth.reset.failure');
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.error.statusCode === INVALID_TOKEN) {
          this.toastHandlingService.handleWarning('auth.reset.invalid-token');
        } else {
          this.toastHandlingService.handleCommonError();
        }
        return of(undefined);
      }),
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

  private createMockResponse(
    token: string,
  ): Observable<BaseResponseModel<LoginResponseModel>> {
    return of({
      statusCode: 200,
      message: 'Token refreshed successfully',
      data: {
        accessToken: token,
        refreshToken: '',
        expiresIn: '3600',
      },
    } as BaseResponseModel<LoginResponseModel>);
  }

  private handleRefreshSuccess(
    response: BaseResponseModel<LoginResponseModel>,
  ): void {
    if (!response.data) {
      throw new Error(response.message ?? 'Invalid refresh token response');
    }

    const { accessToken, refreshToken, expiresIn } = response.data;
    this.saveLocalData(accessToken, refreshToken, expiresIn);
    this.currentRefreshToken.set(accessToken);
  }

  private handleRefreshError(error: any): Observable<never> {
    if (error.status === 401) {
      this.deleteToken();
      this.router.navigate(['/auth/login']);
    }
    this.currentRefreshToken.set(null);
    return throwError(() => error);
  }

  private setRefreshingState(isRefreshing: boolean, token: string | null) {
    Promise.resolve().then(() => {
      this.isRefreshing.set(isRefreshing);
      this.currentRefreshToken.set(token);
    });
  }
}
