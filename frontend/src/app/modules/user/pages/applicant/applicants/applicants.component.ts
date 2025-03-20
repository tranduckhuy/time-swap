import {
  Component,
  DestroyRef,
  inject,
  OnInit,
  signal,
  computed,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

import { forkJoin } from 'rxjs';

import { TranslateModule } from '@ngx-translate/core';

import { ApplicantCardComponent } from './applicant-card/applicant-card.component';
import { BannerComponent } from '../../../../../shared/components/banner/banner.component';
import { BreadcrumbComponent } from '../../../../../shared/components/breadcrumb/breadcrumb.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { PreLoaderComponent } from '../../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../../shared/components/toast/toast.component';
import { NiceSelectComponent } from '../../../../../shared/components/nice-select/nice-select.component';

import { SUCCESS_CODE } from '../../../../../shared/constants/status-code-constants';
import { PAGE_SIZE_APPLICANTS } from '../../../../../shared/constants/page-constants';

import { ApplicantsService } from '../applicants.service';
import { IndustryService } from '../../../../../shared/services/industry.service';
import { CategoryService } from '../../../../../shared/services/category.service';
import { LocationService } from '../../../../../shared/services/location.service';
import { ToastHandlingService } from '../../../../../shared/services/toast-handling.service';

import type { ApplicantModel } from '../../../../../shared/models/entities/applicant.model';
import type { ApplicantsRequestModel } from '../../../../../shared/models/api/request/applicants-request.model';

@Component({
  selector: 'app-applicants',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TranslateModule,
    BannerComponent,
    BreadcrumbComponent,
    ApplicantCardComponent,
    PaginationComponent,
    PreLoaderComponent,
    NiceSelectComponent,
    ToastComponent,
  ],
  templateUrl: './applicants.component.html',
  styleUrl: './applicants.component.css',
})
export class ApplicantsComponent implements OnInit {
  // ? Dependency Injection
  private readonly applicantService = inject(ApplicantsService);
  private readonly industryService = inject(IndustryService);
  private readonly categoryService = inject(CategoryService);
  private readonly locationService = inject(LocationService);
  private readonly toastHandlingService = inject(ToastHandlingService);

  // ? Dependency Injection
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  // ? Form
  form!: FormGroup;

  // ? State Management
  isLoading = signal(false);
  jobId = signal<string>('');

  // ? Data Response
  applicants = signal<ApplicantModel[]>([]);
  totalApplicants = signal<number>(0);
  industries = this.industryService.industries;
  categories = this.categoryService.categories;
  cities = this.locationService.cities;
  wards = this.locationService.wards;

  // ? Pagination
  pageIndex = signal<number>(1);
  pageSize = signal<number>(PAGE_SIZE_APPLICANTS);

  // ? Computed Properties
  industriesName = computed(() => this.industries().map((i) => i.industryName));
  categoriesName = computed(() => this.categories().map((c) => c.categoryName));
  citiesName = computed(() => this.cities().map((c) => c.name));
  wardsName = computed(() => this.wards().map((w) => w.fullLocation));
  start = computed(() =>
    this.totalApplicants() === 0
      ? 0
      : (this.pageIndex() - 1) * this.pageSize() + 1,
  );
  end = computed(() =>
    Math.min(this.pageIndex() * this.pageSize(), this.totalApplicants()),
  );

  ngOnInit(): void {
    const jobId = this.activatedRoute.snapshot.paramMap.get('jobId');
    if (!jobId) {
      this.router.navigateByUrl('/not-found');
      return;
    }

    this.initForm();
    this.initialSelectOptions();

    this.jobId.set(jobId);
    this.search(this.jobId());
  }

  onSearch() {
    this.search(this.jobId());
  }

  onPageChange(page: number) {
    this.search(this.jobId(), page);
  }

  handleSelectChange(field: string, value: string, options: any[]): void {
    const id = this.getOptionId(value, options);

    this.form.get(field)?.setValue(id);

    if (field === 'industryId' && id) {
      const subscription = this.categoryService
        .getCategoriesByIndustryId(+id)
        .subscribe();
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  private initForm() {
    this.form = this.fb.group({
      search: '',
      industryId: 0,
      categoryId: 0,
      cityId: '',
      wardId: '',
    });
  }

  private search(jobId: string, page: number = 1): void {
    this.isLoading.set(true);
    this.pageIndex.set(page);

    const req: ApplicantsRequestModel = {
      ...this.form.value,
      pageIndex: this.pageIndex(),
      pageSize: this.pageSize(),
      isActive: true,
    };

    const subscription = this.applicantService
      .getAllApplicantsByJobId(jobId, req)
      .subscribe({
        next: (res) => {
          if (
            res.statusCode === SUCCESS_CODE &&
            res.data &&
            Array.isArray(res.data.data)
          ) {
            this.applicants.set(res.data.data);
            this.totalApplicants.set(res.data.count);
          } else {
            this.showFetchErrorToast();
          }
        },
        error: () => {
          this.isLoading.set(false);
          this.showFetchErrorToast();
        },
        complete: () => this.isLoading.set(false),
      });
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private initialSelectOptions(): void {
    const subscription = forkJoin([
      this.industryService.getAllIndustries(),
      this.categoryService.getAllCategories(),
      this.locationService.getAllCities(),
      this.locationService.getWardByCityId('0'),
    ]).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private getOptionId(value: string, options: any[]): string {
    return (
      options.find(
        (option) =>
          option.name === value ||
          option.fullLocation === value ||
          option.industryName === value ||
          option.categoryName === value,
      )?.id ||
      (options.some((option) => option.industryName || option.categoryName)
        ? '0'
        : '')
    );
  }

  private showFetchErrorToast(): void {
    this.toastHandlingService.handleError('jobs.notify.fetch-jobs-failed');
  }
}
