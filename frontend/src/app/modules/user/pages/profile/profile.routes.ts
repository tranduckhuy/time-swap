import { Routes } from '@angular/router';

import { MyProfileComponent } from './profile-content/my-profile/my-profile.component';
import { PostedJobsComponent } from './profile-content/posted-jobs/posted-jobs.component';
import { AppliedJobsComponent } from './profile-content/applied-jobs/applied-jobs.component';

export const profileRoutes: Routes = [
    {
        path: '',
        redirectTo: 'my-profile',
        pathMatch: 'full',
    },
    {
        path: 'my-profile',
        component: MyProfileComponent
    },
    {
        path: 'posted-jobs',
        component: PostedJobsComponent
    },
    {
        path: 'applied-jobs',
        component: AppliedJobsComponent
    }
]