import { Component, inject, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';

import { JobPostModel } from '../../models/entities/job.model';

@Component({
  selector: 'app-job-post',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './job-post.component.html',
  styleUrl: './job-post.component.css'
})
export class JobPostComponent {
  job = input.required<JobPostModel>();

  private router = inject(Router);

  navigateToJobPost(jobId: string) {
    this.router.navigate(['/jobs', jobId]).then(() => {
      window.scroll({
        top: 0,
        behavior: 'smooth'
      });
    });
  }
}
