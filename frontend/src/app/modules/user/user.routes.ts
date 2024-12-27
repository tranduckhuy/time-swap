import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { AssigneesComponent } from './pages/assignee/assignees/assignees.component';
import { AssigneeDetailComponent } from './pages/assignee/assignee-detail/assignee-detail.component';

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
    },
    {
        path: 'assignees/:assigneeId',
        component: AssigneeDetailComponent
    }
]