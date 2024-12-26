import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { AuthService } from '../auth/auth.service';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  if (authService.isLoggedIn()) {
    const cloneReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${authService.getToken()}`)
    });
    return next(cloneReq);
  }
  return next(req);
};
