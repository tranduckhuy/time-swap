import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';

import { catchError, finalize, Observable, retry, shareReplay, switchMap, tap, throwError } from 'rxjs';

import { addAuthHeader, handleUnauthorizedError, isPublicPath, retryStrategy } from '../../shared/utils/request-utils';

import { AuthService } from '../auth/auth.service';

import type { RefreshRequestModel } from '../../shared/models/api/request/login-request.model';
import type { LoginResponseModel } from '../../shared/models/api/response/login-response.model';
import type { BaseResponseModel } from '../../shared/models/api/base-response.model';

/**
 * HTTP Interceptor for handling token-based authentication.
 * 
 * @features
 * - Adds authentication token to requests if user is logged in
 * - Automatically refreshes expired tokens
 * - Handles 401 errors by redirecting to login
 * - Prevents multiple parallel refresh token requests
 * - Queues requests while token refresh is in progress
 */
export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  let refreshTokenRequest: Observable<BaseResponseModel<LoginResponseModel>> | null = null;
  const MAX_RETRIES = 3;
  
  // ? Skip authentication for public endpoints
  // ? Can config public endpoints in isPublicPath(url: string) method
  if (isPublicPath(req.url)) {
    return next(req);
  }

  // ? If not logged in, proceed with request but handle 401
  if (!authService.isLoggedIn()) {
    return next(req).pipe(
      retry({
        count: MAX_RETRIES,
        delay: retryStrategy
      }),
      catchError(handleUnauthorizedError(router))
    );
  }

  // ? Add token to request if it is not expired
  if (!authService.isTokenExpired) {
    return next(addAuthHeader(req, authService.accessToken)).pipe(
      retry({
        count: MAX_RETRIES,
        delay: retryStrategy
      }),
      catchError(handleUnauthorizedError(router))
    );
  }

  // ? Token is expired, attempt refresh
  if (!refreshTokenRequest) {
    const refreshReq: RefreshRequestModel = {
      accessToken: authService.accessToken!,
      refreshToken: authService.refreshToken!
    };

    refreshTokenRequest = authService.refreshTokenApi(refreshReq);
  }

  // ? Use cached refresh token request
  return refreshTokenRequest.pipe(
    switchMap(() => {
      return next(addAuthHeader(req, authService.accessToken)).pipe(
        retry({
          count: MAX_RETRIES,
          delay: retryStrategy
        })
      );
    }),
    catchError(error => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        authService.logout();
        router.navigate(['/auth/login']);
      }
      return throwError(() => error);
    })
  );
};