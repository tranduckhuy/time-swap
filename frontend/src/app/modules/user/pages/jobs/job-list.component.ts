import { Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';

import { TranslateModule } from '@ngx-translate/core';

import { forkJoin, map } from 'rxjs';

import { JobPostComponent } from '../../../../shared/components/job-post/job-post.component';
import { BannerComponent } from "../../../../shared/components/banner/banner.component";
import { BreadcrumbComponent } from "../../../../shared/components/breadcrumb/breadcrumb.component";
import { PaginationComponent } from "../../../../shared/components/pagination/pagination.component";
import { NiceSelectComponent } from "../../../../shared/components/nice-select/nice-select.component";
import { ToastComponent } from "../../../../shared/components/toast/toast.component";
import { PreLoaderComponent } from "../../../../shared/components/pre-loader/pre-loader.component";

import { 
  createFilterOptions, 
  createPostedDateOptions 
} from '../../../../shared/utils/util-functions';

import { PAGE_SIZE_JOBS } from '../../../../shared/constants/page-constants';
import { SUCCESS_CODE } from '../../../../shared/constants/status-code-constants';

import { JobsService } from './jobs.service';
import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

import type { JobPostModel } from '../../../../shared/models/entities/job.model';
import type { IndustryModel } from '../../../../shared/models/entities/industry.model';
import type { CategoryModel } from '../../../../shared/models/entities/category.model';
import type { CityModel, WardModel } from '../../../../shared/models/entities/location.model';
import type { JobListRequestModel } from '../../../../shared/models/api/request/job-list-request.model';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [
    TranslateModule,
    ReactiveFormsModule,
    JobPostComponent,
    BannerComponent,
    BreadcrumbComponent,
    PaginationComponent,
    NiceSelectComponent,
    ToastComponent,
    PreLoaderComponent,
  ],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css',
})
export class JobListComponent implements OnInit {
  // ? Form Properties
  form!: FormGroup;

  // ? State Management
  isLoading = signal(false);

  // ? Data For Select Options
  industries = signal<IndustryModel[]>([]);
  categories = signal<CategoryModel[]>([]);
  cities = signal<CityModel[]>([]);
  wards = signal<WardModel[]>([]);
  postedDate = signal<string[]>([]);

  // ? Data Response
  jobs = signal<JobPostModel[]>([]);
  totalJobs = signal(0);
  pageIndex = signal(1);
  pageSize = signal(PAGE_SIZE_JOBS);

  // ? Computed Properties
  industriesName = computed(() => this.industries().map(i => i.industryName));
  categoriesName = computed(() => this.categories().map(c => c.categoryName));
  citiesName = computed(() => this.cities().map(c => c.name));
  wardsName = computed(() => this.wards().map(w => w.fullLocation));
  start = computed(() => this.totalJobs() === 0 ? 0 : (this.pageIndex() - 1) * this.pageSize() + 1);
  end = computed(() => Math.min(this.pageIndex() * this.pageSize(), this.totalJobs()));

  // ? Dependency Injection
  private readonly jobsService = inject(JobsService);
  private readonly toastHandlingService = inject(ToastHandlingService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  ngOnInit(): void {
    this.initialForm();
    this.initialSelectOptions();
    this.search();
  }

  onSearch(): void {
    this.search();
  }

  onPageChange(page: number): void {
    this.search(page);
  }

  handleSelectChange(field: string, value: string, options: any[]): void {
    const id = options.find(
      option => option.name === value || 
      option.industryName === value || 
      option.categoryName === value
    )?.id || '';
    
    this.form.get(field)?.setValue(id);
    
    if (field === 'cityId' && id) 
      this.fetchWardsByCityId(id);
  }

  private initialForm(): void {
    this.form = this.fb.group({
      search: '',
      minFee: '',
      maxFee: '',
      industryId: 0,
      categoryId: 0,
      cityId: '',
      wardId: '',
      postedDate: '',
    });
  }

  private initialSelectOptions(): void {
    this.setDefaultSelectOptions();

    const subscription = forkJoin([
      this.jobsService.getAllIndustries(),
      this.jobsService.getAllCategories(),
      this.jobsService.getAllCities(),
    ])
    .pipe(
      map(([industryRes, categoryRes, cityRes]) => ({
        industries: [this.industries()[0], ...(industryRes.data || [])],
        categories: [this.categories()[0], ...(categoryRes.data || [])],
        cities: [this.cities()[0], ...(cityRes.data || [])],
      }))
    )
    .subscribe({
      next: (data) => {
        this.industries.set(data.industries);
        this.categories.set(data.categories);
        this.cities.set(data.cities);
      },
      error: () => this.showErrorToast(),
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private search(page: number = 1): void {
    this.isLoading.set(true);
    this.pageIndex.set(page);

    const req: JobListRequestModel = {
      ...this.form.value,
      pageIndex: this.pageIndex(),
      pageSize: this.pageSize(),
      isActive: true,
    };

    const subscription = this.jobsService.getAllJobs(req).subscribe({
      next: (res) => {
        if (res.statusCode === SUCCESS_CODE && res.data) {
          const { data, count  } = res.data;
          this.jobs.set(data.filter(job => job.ward !== null));
          this.totalJobs.set(count);
        } else {
          this.resetJobs();
          this.showFetchErrorToast();
        }
      },
      error: () => {
        this.isLoading.set(false);
        this.resetJobs();
        this.showFetchErrorToast();
      },
      complete: () => this.isLoading.set(false),
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private fetchWardsByCityId(cityId: string): void {
    const subscription = this.jobsService.getWardByCityId(cityId).subscribe({
      next: (res) => {
        if (res.statusCode === SUCCESS_CODE) {
          this.wards.set(res.data || []);
        } else {
          this.toastHandlingService.handleError('jobs.notify.fetch-wards-failed');
        }
      },
      error: () => this.showErrorToast(),
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private setDefaultSelectOptions(): void {
    const defaultFilterOptions = createFilterOptions(this.multiLanguageService);
    const defaultPostedDateOptions = createPostedDateOptions(this.multiLanguageService);

    this.industries.set([defaultFilterOptions.industries]);
    this.categories.set([defaultFilterOptions.categories]);
    this.cities.set([defaultFilterOptions.cities]);
    this.wards.set([defaultFilterOptions.wards]);
    this.postedDate.set(defaultPostedDateOptions);
  }

  private resetJobs(): void {
    this.jobs.set([]);
    this.totalJobs.set(0);
    this.pageIndex.set(1);
  }

  private showErrorToast(): void {
    this.toastHandlingService.handleError('common.notify.error-message');
  }

  private showFetchErrorToast(): void {
    this.toastHandlingService.handleError('jobs.notify.fetch-jobs-failed');
  }
}