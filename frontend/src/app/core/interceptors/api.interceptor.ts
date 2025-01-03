import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { switchMap, throwError } from 'rxjs';

import { AuthService } from '../auth/auth.service';

import type { RefreshRequestModel } from '../../shared/models/api/request/login-request.model';

/**
 * HTTP Interceptor for handling token-based authentication.
 * 
 * This interceptor checks if the token is expired. If not, it adds the token
 * to the request headers. If the token is expired, it tries to refresh it 
 * using the refresh token and updates the request with the new access token.
 */
export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  if (!authService.isTokenExpired()) {
    const cloneReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${authService.getToken()}`)
    });

    return next(cloneReq);
  } else {
    const accessToken = authService.getToken();
    const refreshToken = authService.getRefreshToken();
    const refreshReq: RefreshRequestModel = {
      accessToken: accessToken!,
      refreshToken: refreshToken!
    }
    
    return authService.refreshToken(refreshReq).pipe(
      switchMap((res) => {
        const data = res.data;
        if (data) {
          authService.saveLocalData(data.accessToken, data.refreshToken, data.expiresIn);
          
          const cloneReq = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${authService.getToken()}`)
          });

          return next(cloneReq);
        }

        return throwError(() => new Error('Refresh token invalid!'));
      })
    );
  }
};
