import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter, withComponentInputBinding, withRouterConfig } from '@angular/router';

import { apiInterceptor } from './core/interceptors/api.interceptor';

import { defaultLayoutRoutes, authLayoutRoutes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(withInterceptors([apiInterceptor])),
    provideAnimationsAsync(),
    provideRouter(
      [...defaultLayoutRoutes, ...authLayoutRoutes], 
      withComponentInputBinding(), 
      withRouterConfig({
        paramsInheritanceStrategy: 'always',
      }
    ),
  )]
};
