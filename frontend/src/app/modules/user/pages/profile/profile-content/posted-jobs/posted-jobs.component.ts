import { Component, DestroyRef, effect, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { JobPostComponent } from '../../../../../../shared/components/job-post/job-post.component';
import { PreLoaderComponent } from '../../../../../../shared/components/pre-loader/pre-loader.component';
import { EmptyJobComponent } from '../../../../../../shared/components/empty-job/empty-job.component';

import { ProfileService } from '../../profile.service';

@Component({
  selector: 'app-posted-jobs',
  standalone: true,
  imports: [JobPostComponent, PreLoaderComponent, EmptyJobComponent],
  templateUrl: './posted-jobs.component.html',
  styleUrl: './posted-jobs.component.css',
})
export class PostedJobsComponent implements OnInit {
  // Dependency Injection
  private readonly destroyRef = inject(DestroyRef);
  private readonly profileService = inject(ProfileService);

  // State Management
  user = this.profileService.user;
  jobs = this.profileService.jobs;
  isLoading = this.profileService.isLoading;

  ngOnInit(): void {
    this.profileService
      .getUserProfile()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe();

    effect(() => {
      const currentUser = this.user();
      if (currentUser) {
        this.profileService
          .getJobPostsByUserId(false, currentUser.id)
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe();
      }
    });
  }
}
