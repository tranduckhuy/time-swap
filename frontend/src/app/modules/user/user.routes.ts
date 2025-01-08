import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';

import { of } from 'rxjs';

import { HomeComponent } from './pages/home/home.component';
import { AboutComponent } from './pages/about/about.component';
import { JobListComponent } from './pages/jobs/job-list.component';
import { JobDetailComponent } from './pages/jobs/job-detail/job-detail.component';
import { PostJobComponent } from './pages/jobs/post-job/post-job.component';
import { AssigneesComponent } from './pages/assignee/assignees/assignees.component';
import { AssigneeDetailComponent } from './pages/assignee/assignee-detail/assignee-detail.component';
import { ContactComponent } from './pages/contact/contact.component';
import { ProfileComponent } from './pages/profile/profile.component';

import { authGuard } from '../../core/auth/auth.guard';

import { JobsService } from './pages/jobs/jobs.service';

import type { JobDetailResponseModel } from '../../shared/models/api/response/jobs-response.model';

const jobDetailResolver: ResolveFn<JobDetailResponseModel | null> = (activatedRoute) => {
    const jobsService = inject(JobsService);
    const jobId = activatedRoute.paramMap.get('jobId');

    if (!jobId) {
        return of(null);
    }

    return jobsService.getJobDetailById(jobId);
};

export const userRoutes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'prefix'
    },
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'about',
        component: AboutComponent
    },
    {
        path: 'jobs',
        component: JobListComponent
    },
    {
        path: 'jobs/:jobId',
        component: JobDetailComponent,
        canMatch: [authGuard],
        resolve: {
            job: jobDetailResolver
        }
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
        loadChildren: () => import('./pages/profile/profile.routes').then(mod => mod.profileRoutes)
    }
]