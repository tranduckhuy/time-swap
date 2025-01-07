import { Component } from '@angular/core';

import { JobPostComponent } from "../../../../../../shared/components/job-post/job-post.component";

@Component({
  selector: 'app-posted-jobs',
  standalone: true,
  imports: [JobPostComponent],
  templateUrl: './posted-jobs.component.html',
  styleUrl: './posted-jobs.component.css'
})
export class PostedJobsComponent {
}
