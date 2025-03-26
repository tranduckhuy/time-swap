import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';

import { AuthService } from './auth.service';
import { ToastHandlingService } from '../../shared/services/toast-handling.service';

/**
 * Guard to protect routes that require authentication.
 *
 * - Checks if the user is logged in using `AuthService.isLoggedIn()`.
 * - If not logged in, redirects to `/auth/login` and prevents access.
 *
 * @returns {boolean} `true` if the user is authenticated, otherwise `false`.
 */
export const authGuard: CanMatchFn = () => {
  const router = inject(Router);
  const authService = inject(AuthService);

  if (!authService.isLoggedIn()) {
    router.navigate(['/auth/login']);
    return false;
  }

  return true;
};

/**
 * Guard to protect the reset password route.
 *
 * - Extracts `token` and `email` from the URL query parameters.
 * - If either parameter is missing, redirects to `/auth/forgot-password` and denies access.
 *
 * @returns {boolean} `true` if both `token` and `email` exist on URL, otherwise `false`.
 */
export const resetPasswordGuard: CanMatchFn = () => {
  const router = inject(Router);
  const toastHandlingService = inject(ToastHandlingService);

  const queryParams = new URLSearchParams(location.search);
  const token = queryParams.get('token');
  const email = queryParams.get('email');

  if (!token || !email) {
    toastHandlingService.handleWarning('auth.reset.invalid-token');
    router.navigate(['/auth/forgot-password']);
    return false;
  }

  return true;
};

/**
 * Guard to protect login and register routes.
 *
 * - Checks if the user is logged in using `AuthService.isLoggedIn()`.
 * - If logged in, redirects to `/home`.
 *
 * @returns {boolean} `true` if the user is authenticated, otherwise `false`.
 */
export const loggedInGuard: CanMatchFn = () => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const toastHandlingService = inject(ToastHandlingService);

  if (authService.isLoggedIn()) {
    toastHandlingService.handleWarning('auth.login.still-have-session');
    router.navigate(['/home']);
    return false;
  }

  return true;
};
