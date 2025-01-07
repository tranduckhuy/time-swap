import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';

import { of } from 'rxjs';

import { HomeComponent } from './pages/home/home.component';
import { JobListComponent } from './pages/jobs/job-list.component';
import { JobDetailComponent } from './pages/jobs/job-detail/job-detail.component';
import { PostJobComponent } from './pages/jobs/post-job/post-job.component';
import { ContactComponent } from './pages/contact/contact.component';
import { ProfileComponent } from './pages/profile/profile.component';

import { authGuard } from '../../core/auth/auth.guard';

import { JobsService } from './pages/jobs/jobs.service';

import type { JobDetailResponseModel } from '../../shared/models/api/response/jobs-response.model';

import { ApplicantsComponent } from './pages/applicant/applicants/applicants.component';
import { ApplicantDetailComponent } from './pages/applicant/applicant-detail/applicant-detail.component';

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
        path: 'jobs-detail',
        component: JobDetailComponent,
        data: {
            job: {
                "id": "2c2c2c2c-2c2c-2c2c-2c2c-2c2c2c2c2c2c",
                "userId": "2a2a2a2a-2a2a-2a2a-2a2a-2a2a2a2a2a2a",
                "title": "Code hộ",
                "description": "Cần code hộ backend cho 1 ứng dụng di động",
                "fee": 200,
                "startDate": null,
                "dueDate": "2025-01-30T08:31:04.768793Z",
                "assignedTo": null,
                "isOwnerCompleted": false,
                "isAssigneeCompleted": false,
                "category": {
                  "id": 1,
                  "categoryName": "Code hộ"
                },
                "industry": {
                    "id": 1,
                    "industryName": "Công Nghệ Thông Tin"
                },
                "ward": {
                  "id": "21550",
                  "name": "Nhơn Bình",
                  "fullLocation": "Phường Nhơn Bình, Quy Nhơn, Bình Định"
                },
                "createdAt": "2024-12-31T08:31:04.768792Z",
                "modifiedAt": "0001-01-01T00:00:00"
              },
        }
    },
    {
        path: 'post-job',
        component: PostJobComponent
    },
    {
        path: 'applicants/:jobId',
        component: ApplicantsComponent,
    },
    {
        path: 'applicants/:jobId/:applicantId',
        component: ApplicantDetailComponent
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