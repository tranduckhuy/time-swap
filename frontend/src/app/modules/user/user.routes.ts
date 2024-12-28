import { Routes } from '@angular/router';

import { HomeComponent } from './pages/home/home.component';
import { JobListComponent } from './pages/jobs/job-list/job-list.component';
import { JobDetailComponent } from './pages/jobs/job-detail/job-detail.component';
import { PostJobComponent } from './pages/jobs/post-job/post-job.component';
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
        path: 'jobs',
        component: JobListComponent
    },
    {
        // path: 'jobs/:id',
        path: 'job-detail',
        component: JobDetailComponent
    },
    {
        path: 'post-job',
        component: PostJobComponent
    },
    {
        path: 'assignees',
        component: AssigneesComponent
    }
]