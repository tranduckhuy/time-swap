import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpInterceptorFn } from '@angular/common/http';

import { catchError, retry, switchMap, throwError } from 'rxjs';

import {
  addAuthHeader,
  handleUnauthorizedError,
  isPublicPath,
  retryStrategy,
} from '../../shared/utils/request-utils';

import { AuthService } from '../auth/auth.service';

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
  const MAX_RETRY_ATTEMPTS = 3;

  if (isPublicPath(req.url)) {
    return next(req);
  }

  if (!authService.isLoggedIn()) {
    return next(req).pipe(catchError(handleUnauthorizedError(router)));
  }

  if (!authService.isTokenExpired) {
    return next(addAuthHeader(req, authService.accessToken)).pipe(
      catchError(handleUnauthorizedError(router)),
    );
  }

  // Avoid recursive calls by checking if the request is for token refresh
  if (req.url.includes('/refresh-token')) {
    return next(req);
  }

  // Refresh token logic
  return authService
    .refreshTokenApi({
      accessToken: authService.accessToken!,
      refreshToken: authService.refreshToken!,
    })
    .pipe(
      retry({ count: MAX_RETRY_ATTEMPTS, delay: retryStrategy }), // Retry token refresh up to 3 times
      switchMap((response: BaseResponseModel<LoginResponseModel>) => {
        if (!response.data) {
          return throwError(
            () => new Error('No data returned from refresh token API'),
          );
        }

        const { accessToken } = response.data;
        const clonedRequest = addAuthHeader(req, accessToken);
        return next(clonedRequest);
      }),
      catchError((error) => {
        authService.deleteToken();
        router.navigate(['/auth/login']);
        return throwError(() => error);
      }),
    );
};
