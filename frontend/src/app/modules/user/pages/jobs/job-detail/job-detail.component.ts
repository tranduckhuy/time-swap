import { Component } from '@angular/core';

import { DUMMY_JOBS } from '../job-list/dummy-job';

import { JobPostComponent } from "../../../../../shared/components/job-post/job-post.component";
import { GridLayoutComponent } from "../../../../../core/layout/grid-layout/grid-layout.component";
import { JobAlertComponent } from "../../../../../shared/components/job-alert/job-alert.component";
import { ButtonWithIconComponent } from "../../../../../shared/components/button-with-icon/button-with-icon.component";

@Component({
  selector: 'app-job-detail',
  standalone: true,
  imports: [JobPostComponent, GridLayoutComponent, JobAlertComponent, ButtonWithIconComponent],
  templateUrl: './job-detail.component.html',
  styleUrl: './job-detail.component.css'
})
export class JobDetailComponent {
  relatedJobs = DUMMY_JOBS.slice(0, 3);
}
