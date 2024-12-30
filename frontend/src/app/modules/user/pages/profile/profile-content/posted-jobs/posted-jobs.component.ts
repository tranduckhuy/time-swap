import { Component } from '@angular/core';
import { DUMMY_JOBS } from '../../../jobs/job-list/dummy-job';
import { JobPostComponent } from "../../../../../../shared/components/job-post/job-post.component";

@Component({
  selector: 'app-posted-jobs',
  standalone: true,
  imports: [JobPostComponent],
  templateUrl: './posted-jobs.component.html',
  styleUrl: './posted-jobs.component.css'
})
export class PostedJobsComponent {
  postedJobs = DUMMY_JOBS.slice(0, 3);

}
