import { Routes } from '@angular/router';

import { DefaultLayoutComponent } from './core/layout/default-layout/default-layout.component';
import { AuthLayoutComponent } from './core/layout/auth-layout/auth-layout.component';

export const defaultLayoutRoutes: Routes = [
    {
        path: '',
        component: DefaultLayoutComponent,
        loadChildren: () => import('./modules/user/user.routes').then(mod => mod.userRoutes)
    }
];

export const noHeaderFooterLayoutRoutes: Routes = [
    {
        path: 'auth',
        component: AuthLayoutComponent,
        loadChildren: () => import('./core/auth/auth.routes').then(mod => mod.authRoutes)
    },
    {
        path: 'coming-soon',
        loadComponent: () => import('./shared/components/coming-soon/coming-soon.component').then(mod => mod.ComingSoonComponent)
    },
    {
        path: 'not-found',
        loadComponent: () => import('./shared/components/not-found/not-found.component').then(mod => mod.NotFoundComponent)
    },
    {
        path: '**',
        loadComponent: () => import('./shared/components/not-found/not-found.component').then(mod => mod.NotFoundComponent)
    }
]
