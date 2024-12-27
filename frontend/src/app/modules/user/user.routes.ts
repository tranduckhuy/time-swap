import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { AssigneesComponent } from './pages/assignee/assignees/assignees.component';

export const userRoutes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'assignees',
        component: AssigneesComponent
    }
]