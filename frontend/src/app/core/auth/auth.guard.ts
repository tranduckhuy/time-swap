import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';

import { AuthService } from './auth.service';

export const authGuard: CanMatchFn = () => {
  const authService = inject(AuthService);

  if (!authService.isLoggedIn()) {
    const router = inject(Router);
    router.navigate(['/auth/login']);
    return false;
  }

  return true;
};
