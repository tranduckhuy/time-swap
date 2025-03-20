import {
  Component,
  DestroyRef,
  inject,
  input,
  OnInit,
  signal,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { GridLayoutComponent } from '../../../../../core/layout/grid-layout/grid-layout.component';
import { ButtonWithIconComponent } from '../../../../../shared/components/button-with-icon/button-with-icon.component';
import { BannerComponent } from '../../../../../shared/components/banner/banner.component';
import { BannerDetailComponent } from '../../../../../shared/components/banner/banner-detail/banner-detail.component';
import { PreLoaderComponent } from '../../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../../shared/components/toast/toast.component';

import { JobsService } from '../../jobs/jobs.service';

import type { UserModel } from '../../../../../shared/models/entities/user.model';
import type { AssignJobRequestModel } from '../../../../../shared/models/api/request/assign-job-request.model';

@Component({
  selector: 'app-applicant-detail',
  standalone: true,
  imports: [
    BannerComponent,
    BannerDetailComponent,
    GridLayoutComponent,
    ButtonWithIconComponent,
    TranslateModule,
    PreLoaderComponent,
    ToastComponent,
  ],
  templateUrl: './applicant-detail.component.html',
  styleUrl: './applicant-detail.component.css',
})
export class ApplicantDetailComponent implements OnInit {
  private readonly jobsService = inject(JobsService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);

  user = input.required<UserModel>();

  isLoading = signal<boolean>(true);
  jobId = signal<string | null>(null);

  ngOnInit(): void {
    if (!this.user) {
      this.router.navigateByUrl('/not-found');
    }

    this.activatedRoute.paramMap.subscribe((params) => {
      this.jobId.set(params.get('jobId'));
    });

    const timeOutId = setTimeout(() => this.isLoading.set(false), 800);
    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }

  assignJob(userId: string) {
    const req: AssignJobRequestModel = {
      jobPostId: this.jobId() ?? '',
      userAppliedId: userId,
    };

    const subscription = this.jobsService.assignJob(req).subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
