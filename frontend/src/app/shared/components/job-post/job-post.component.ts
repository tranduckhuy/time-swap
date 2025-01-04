import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';

import { JobPostModel } from '../../models/entities/job.model';

@Component({
  selector: 'app-job-post',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './job-post.component.html',
  styleUrl: './job-post.component.css'
})
export class JobPostComponent {
  job = input.required<JobPostModel>();
}
