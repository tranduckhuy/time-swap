import { Routes } from "@angular/router";

export const authRoutes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('./pages/login/login.component').then(mod => mod.LoginComponent)
    },
    {
        path: 'register',
        loadComponent: () => import('./pages/register/register.component').then(mod => mod.RegisterComponent)
    },
    {
        path: 'password-recovery',
        loadComponent: () => import('./pages/password-recovery/password-recovery.component').then(mod => mod.PasswordRecoveryComponent)
    },

]