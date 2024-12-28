import { Component, input } from '@angular/core';

type Job = {
  title: string,
  location: string,
  postedTime: string,
  description: string,
  category: string,
  jobType: string,
  image: string,
  link: string
}

@Component({
  selector: 'app-job-post',
  standalone: true,
  imports: [],
  templateUrl: './job-post.component.html',
  styleUrl: './job-post.component.css'
})
export class JobPostComponent {
  job = input.required<Job>();
}
