import { Component, inject, OnInit } from '@angular/core';
import { JobPostComponent } from "../../../../../../shared/components/job-post/job-post.component";
import { ProfileService } from '../../profile.service';
import { PreLoaderComponent } from "../../../../../../shared/components/pre-loader/pre-loader.component";
import { EmptyJobComponent } from "../../../../../../shared/components/empty-job/empty-job.component";

@Component({
  selector: 'app-posted-jobs',
  standalone: true,
  imports: [JobPostComponent, PreLoaderComponent, EmptyJobComponent],
  templateUrl: './posted-jobs.component.html',
  styleUrl: './posted-jobs.component.css'
})
export class PostedJobsComponent implements OnInit {
  // Dependency Injection
  private readonly profileService = inject(ProfileService);

  // State Management
  user = this.profileService.user;
  jobs = this.profileService.jobs;
  isLoading = this.profileService.isLoading;

  ngOnInit(): void {
    this.profileService.getJobPostsByUserId(false, this.user()!.id).subscribe()
  }
}
