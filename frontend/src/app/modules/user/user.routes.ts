import { inject } from '@angular/core';
import { ResolveFn, Routes } from '@angular/router';

import { of } from 'rxjs';

import { HomeComponent } from './pages/home/home.component';
import { AboutComponent } from './pages/about/about.component';
import { JobListComponent } from './pages/jobs/job-list.component';
import { JobDetailComponent } from './pages/jobs/job-detail/job-detail.component';
import { PostJobComponent } from './pages/jobs/post-job/post-job.component';
import { ContactComponent } from './pages/contact/contact.component';
import { PaymentComponent } from './pages/payment/payment.component';
import { ProfileComponent } from './pages/profile/profile.component';

import { authGuard } from '../../core/auth/auth.guard';

import { JobsService } from './pages/jobs/jobs.service';

import { ApplicantsComponent } from './pages/applicant/applicants/applicants.component';
import { ApplicantDetailComponent } from './pages/applicant/applicant-detail/applicant-detail.component';

import { ApplicantsService } from './pages/applicant/applicants.service';

import type { JobDetailResponseModel } from '../../shared/models/api/response/jobs-response.model';
import { UserModel } from '../../shared/models/entities/user.model';

const jobDetailResolver: ResolveFn<JobDetailResponseModel | null> = (
  activatedRoute,
) => {
  const jobsService = inject(JobsService);
  const jobId = activatedRoute.paramMap.get('jobId');

  if (!jobId) {
    return of(null);
  }

  return jobsService.getJobDetailById(jobId);
};

const applicantsDetailResolver: ResolveFn<UserModel | null> = (
  activatedRoute,
) => {
  const applicantsService = inject(ApplicantsService);
  const applicantId = activatedRoute.paramMap.get('applicantId');

  if (!applicantId) {
    return of(null);
  }

  return applicantsService.getApplicantDetailById(applicantId);
};

export const userRoutes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'prefix',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'about',
    component: AboutComponent,
  },
  {
    path: 'jobs',
    component: JobListComponent,
  },
  {
    path: 'jobs/:jobId',
    component: JobDetailComponent,
    canMatch: [authGuard],
    resolve: {
      job: jobDetailResolver,
    },
  },
  {
    path: 'post-job',
    component: PostJobComponent,
  },
  {
    path: 'applicants/:jobId',
    component: ApplicantsComponent,
  },
  {
    path: 'applicants/:jobId/:applicantId',
    component: ApplicantDetailComponent,
    resolve: {
      user: applicantsDetailResolver,
    },
  },
  {
    path: 'contact',
    component: ContactComponent,
  },
  {
    path: 'payment',
    component: PaymentComponent,
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canMatch: [authGuard],
    loadChildren: () =>
      import('./pages/profile/profile.routes').then((mod) => mod.profileRoutes),
  },
];
