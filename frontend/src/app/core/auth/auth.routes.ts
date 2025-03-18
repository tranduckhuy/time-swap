import { Routes } from '@angular/router';

import { loggedInGuard, resetPasswordGuard } from './auth.guard';

export const authRoutes: Routes = [
  {
    path: 'login',
    canMatch: [loggedInGuard],
    loadComponent: () =>
      import('./pages/login/login.component').then((mod) => mod.LoginComponent),
  },
  {
    path: 'register',
    canMatch: [loggedInGuard],
    loadComponent: () =>
      import('./pages/register/register.component').then(
        (mod) => mod.RegisterComponent,
      ),
  },
  {
    path: 'forgot-password',
    canMatch: [loggedInGuard],
    loadComponent: () =>
      import('./pages/forgot-password/forgot-password.component').then(
        (mod) => mod.ForgotPasswordComponent,
      ),
  },
  {
    path: 'reset-password',
    canMatch: [resetPasswordGuard],
    loadComponent: () =>
      import('./pages/reset-password/reset-password.component').then(
        (mod) => mod.ResetPasswordComponent,
      ),
  },
];
