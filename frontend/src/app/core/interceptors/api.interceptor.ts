import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { switchMap, throwError } from 'rxjs';

import { AuthService } from '../auth/auth.service';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  if (!authService.isTokenExpired()) {
    const cloneReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${authService.getToken()}`)
    });

    return next(cloneReq);
  } else {
    return authService.refreshToken().pipe(
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
