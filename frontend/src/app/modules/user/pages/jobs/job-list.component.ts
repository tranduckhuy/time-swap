import {
  Component,
  DestroyRef,
  OnInit,
  computed,
  inject,
  signal,
} from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';

import { TranslateModule } from '@ngx-translate/core';

import { forkJoin } from 'rxjs';

import { JobPostComponent } from '../../../../shared/components/job-post/job-post.component';
import { BannerComponent } from '../../../../shared/components/banner/banner.component';
import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { NiceSelectComponent } from '../../../../shared/components/nice-select/nice-select.component';
import { ToastComponent } from '../../../../shared/components/toast/toast.component';
import { PreLoaderComponent } from '../../../../shared/components/pre-loader/pre-loader.component';

import {
  createPostedDateOptions,
  fetchWardsByCityId,
} from '../../../../shared/utils/util-functions';

import { PAGE_SIZE_JOBS } from '../../../../shared/constants/page-constants';

import { JobsService } from './jobs.service';
import { IndustryService } from '../../../../shared/services/industry.service';
import { CategoryService } from '../../../../shared/services/category.service';
import { LocationService } from '../../../../shared/services/location.service';
import { FilterService } from '../../../../shared/services/filter.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

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

  // ? Dependency Injection
  private readonly jobsService = inject(JobsService);
  private readonly industryService = inject(IndustryService);
  private readonly categoryService = inject(CategoryService);
  private readonly locationService = inject(LocationService);
  private readonly filterService = inject(FilterService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  // ? State Management
  isLoading = this.jobsService.isLoading;

  // ? Pagination
  pageIndex = signal<number>(1);
  pageSize = signal<number>(PAGE_SIZE_JOBS);

  // ? Data For Select Options
  industries = this.industryService.industries;
  categories = this.categoryService.categories;
  cities = this.locationService.cities;
  wards = this.locationService.wards;

  // ? Data Response
  jobs = this.jobsService.jobs;
  totalJobs = this.jobsService.totalJobs;

  // ? Computed Properties
  industriesName = computed(() => this.industries().map((i) => i.industryName));
  categoriesName = computed(() => this.categories().map((c) => c.categoryName));
  citiesName = computed(() => this.cities().map((c) => c.name));
  wardsName = computed(() => this.wards().map((w) => w.fullLocation));
  start = computed(() =>
    this.totalJobs() === 0 ? 0 : (this.pageIndex() - 1) * this.pageSize() + 1,
  );
  end = computed(() =>
    Math.min(this.pageIndex() * this.pageSize(), this.totalJobs()),
  );
  postedDate = computed(() => {
    this.multiLanguageService.language();
    return createPostedDateOptions(this.multiLanguageService);
  });

  ngOnInit(): void {
    const subscription = this.filterService.loadSelectOptions().subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());

    this.initialForm();
    this.search();
  }

  onSearch(): void {
    this.search();
  }

  onPageChange(page: number): void {
    this.search(page);
  }

  handleSelectChange(field: string, value: string, options: any[]): void {
    const id = this.filterService.getOptionId(value, options);
    this.form.get(field)?.setValue(id);
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

  private search(page: number = 1): void {
    this.pageIndex.set(page);

    const req: JobListRequestModel = {
      ...this.form.value,
      pageIndex: this.pageIndex(),
      pageSize: this.pageSize(),
      isActive: true,
    };
    const subscription = this.jobsService.getAllJobs(req).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
