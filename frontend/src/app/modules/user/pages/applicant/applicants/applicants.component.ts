import { Component, DestroyRef, inject, OnInit, signal, computed } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BannerComponent } from '../../../../../shared/components/banner/banner.component';
import { BreadcrumbComponent } from '../../../../../shared/components/breadcrumb/breadcrumb.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { ApplicantCardComponent } from './applicant-card/applicant-card.component';
import { ApplicantsService } from '../applicants.service';
import { SUCCESS_CODE } from '../../../../../shared/constants/status-code-constants';
import { ApplicantModel } from '../../../../../shared/models/entities/applicant.model';
import { ActivatedRoute } from '@angular/router';
import { PreLoaderComponent } from "../../../../../shared/components/pre-loader/pre-loader.component";
import { ToastHandlingService } from '../../../../../shared/services/toast-handling.service';
import { PAGE_SIZE_APPLICANTS } from '../../../../../shared/constants/page-constants';
import { ApplicantsRequestModel } from '../../../../../shared/models/api/request/applicants-request.model';

@Component({
  selector: 'app-applicants',
  standalone: true,
  imports: [BannerComponent, BreadcrumbComponent, ApplicantCardComponent, PaginationComponent, TranslateModule, PreLoaderComponent],
  templateUrl: './applicants.component.html',
  styleUrl: './applicants.component.css'
})
export class ApplicantsComponent implements OnInit {
  // ? State Management
  isLoading = signal(false);
  jobId = signal<string>('');

  // ? Data Response
  applicants = signal<ApplicantModel[]>([]);
  totalApplicants = signal<number>(0);
  pageIndex = signal<number>(1);
  pageSize = signal<number>(PAGE_SIZE_APPLICANTS)

  // ? Computed Properties
  start = computed(() => this.totalApplicants() === 0 ? 0 : (this.pageIndex() - 1) * this.pageSize() + 1);
  end = computed(() => Math.min(this.pageIndex() * this.pageSize(), this.totalApplicants()));

  // ? Dependency Injection
  private readonly applicantService = inject(ApplicantsService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly toastHandlingService = inject(ToastHandlingService);


  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.jobId.set(this.route.snapshot.paramMap.get('jobId')!);
    this.search(this.jobId())
  }

  private search(jobId: string): void {
    this.isLoading.set(true);

    const req: ApplicantsRequestModel = {
      pageIndex: this.pageIndex(),
      pageSize: this.pageSize(),
      isActive: true,
    };

    const subscription = this.applicantService.getAllApplicantsByJobId(jobId, req).subscribe({
      next: (res) => {
        if (res.statusCode === SUCCESS_CODE && res.data && Array.isArray(res.data)) {
          const {data} = res
          this.applicants.set(data)
          this.totalApplicants.set(data.length)
          // const {data, count} = res.data
          // this.totalApplicants.set(count)
          // this.applicants.set(data)
        } else {
          this.showFetchErrorToast();
        }
      },
      error: () => {
        this.isLoading.set(false);
        this.showFetchErrorToast();
      },
      complete: () => this.isLoading.set(false),
    }
  )
  this.destroyRef.onDestroy(() => subscription.unsubscribe())
  }

  private showFetchErrorToast(): void {
    this.toastHandlingService.handleError('jobs.notify.fetch-jobs-failed');
  }
}
