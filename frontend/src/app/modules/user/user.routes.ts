import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';

export const userRoutes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent
    }
]