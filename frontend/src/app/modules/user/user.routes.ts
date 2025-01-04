import { Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';
import { JobListComponent } from './pages/jobs/job-list.component';
import { JobDetailComponent } from './pages/jobs/job-detail/job-detail.component';
import { PostJobComponent } from './pages/jobs/post-job/post-job.component';
import { AssigneesComponent } from './pages/assignee/assignees/assignees.component';
import { AssigneeDetailComponent } from './pages/assignee/assignee-detail/assignee-detail.component';
import { ContactComponent } from './pages/contact/contact.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { profileRoutes } from './pages/profile/profile.routes';

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
        path: 'jobs',
        component: JobListComponent
    },
    {
        path: 'jobs/:id',
        component: JobDetailComponent
    },
    {
        path: 'post-job',
        component: PostJobComponent
    },
    {
        path: 'assignees',
        component: AssigneesComponent
    },
    {
        path: 'assignees/:assigneeId',
        component: AssigneeDetailComponent
    },
    {
        path: 'contact',
        component: ContactComponent
    },
    {
        path: 'profile',
        component: ProfileComponent,
        children: profileRoutes
    }
]