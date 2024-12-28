import { Component } from '@angular/core';
import { DUMMY_JOBS } from '../../../jobs/job-list/dummy-job';
import { JobPostComponent } from "../../../../../../shared/components/job-post/job-post.component";

@Component({
  selector: 'app-applied-jobs',
  standalone: true,
  imports: [JobPostComponent],
  templateUrl: './applied-jobs.component.html',
  styleUrl: './applied-jobs.component.css'
})
export class AppliedJobsComponent {
  appliedJobs = DUMMY_JOBS.slice(0, 3);
}
