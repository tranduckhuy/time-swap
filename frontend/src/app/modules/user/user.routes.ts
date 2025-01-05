import { inject } from '@angular/core';
import { CanActivateFn, ResolveFn, Router, Routes } from '@angular/router';

import { catchError, map, of } from 'rxjs';

import { HomeComponent } from './pages/home/home.component';
import { JobListComponent } from './pages/jobs/job-list.component';
import { JobDetailComponent } from './pages/jobs/job-detail/job-detail.component';
import { PostJobComponent } from './pages/jobs/post-job/post-job.component';
import { AssigneesComponent } from './pages/assignee/assignees/assignees.component';
import { AssigneeDetailComponent } from './pages/assignee/assignee-detail/assignee-detail.component';
import { ContactComponent } from './pages/contact/contact.component';
import { ProfileComponent } from './pages/profile/profile.component';

import { profileRoutes } from './pages/profile/profile.routes';

import { SUCCESS_CODE } from '../../shared/constants/status-code-constants';

import { JobsService } from './pages/jobs/jobs.service';

import { jobDetailCanActivate } from './pages/jobs/jobs.guard';

import type { JobPostModel } from '../../shared/models/entities/job.model';
import { authGuard } from '../../core/auth/auth.guard';

const jobDetailResolver: ResolveFn<JobPostModel | null> = (activatedRoute) => {
    const jobsService = inject(JobsService);
    const jobId = activatedRoute.paramMap.get('jobId');

    if (!jobId) {
        return of(null);
    }

    return jobsService.getJobDetailById(jobId).pipe(
        map((res) => (res.statusCode === SUCCESS_CODE && res.data ? res.data : null)),
        catchError(() => of(null))
    );
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
        path: 'jobs',
        component: JobListComponent
    },
    {
        path: 'jobs/:jobId',
        component: JobDetailComponent,
        canMatch: [authGuard],
        canActivate: [jobDetailCanActivate],
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
        children: profileRoutes
    }
]