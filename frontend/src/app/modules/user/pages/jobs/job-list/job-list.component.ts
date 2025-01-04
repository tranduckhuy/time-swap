import { Component } from '@angular/core';

import { DUMMY_JOBS } from './dummy-job';

import { JobPostComponent } from '../../../../../shared/components/job-post/job-post.component';
import { GridLayoutComponent } from "../../../../../core/layout/grid-layout/grid-layout.component";
import { JobAlertComponent } from "../../../../../shared/components/job-alert/job-alert.component";
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { BreadcrumbComponent } from "../../../../../shared/components/breadcrumb/breadcrumb.component";
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [JobPostComponent, GridLayoutComponent, JobAlertComponent, BannerComponent, BreadcrumbComponent, TranslateModule],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css'
})
export class JobListComponent {
  dummyJobs = DUMMY_JOBS;
}
