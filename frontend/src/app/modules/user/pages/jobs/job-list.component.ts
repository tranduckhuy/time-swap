import { Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';

import { TranslateModule } from '@ngx-translate/core';

import { forkJoin } from 'rxjs';

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

import { JobsService } from './jobs.service';
import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';
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
  pageIndex = signal(1);
  pageSize = signal(PAGE_SIZE_JOBS);

  // ? Dependency Injection
  private readonly jobsService = inject(JobsService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  // ? State Management
  isLoading = this.jobsService.isLoading;

  // ? Data For Select Options
  industries = this.jobsService.industries;
  categories = this.jobsService.categories; 
  cities = this.jobsService.cities;
  wards = this.jobsService.wards;
  postedDate = signal<string[]>(createPostedDateOptions(this.multiLanguageService));

  // ? Data Response
  jobs = this.jobsService.jobs;
  totalJobs = this.jobsService.totalJobs;

  // ? Computed Properties
  industriesName = computed(() => this.industries().map(i => i.industryName));
  categoriesName = computed(() => this.categories().map(c => c.categoryName));
  citiesName = computed(() => this.cities().map(c => c.name));
  wardsName = computed(() => this.wards().map(w => w.fullLocation));
  start = computed(() => this.totalJobs() === 0 ? 0 : (this.pageIndex() - 1) * this.pageSize() + 1);
  end = computed(() => Math.min(this.pageIndex() * this.pageSize(), this.totalJobs()));

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
    const id = field === 'postedDate' ? this.getPostedDateId(value) : this.getOptionId(value, options);
    
    this.form.get(field)?.setValue(id);

    if (field === 'cityId' && id) {
        this.fetchWardsByCityId(id);
    }
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
    const subscription = forkJoin([
      this.jobsService.getAllIndustries(),
      this.jobsService.getAllCategories(),
      this.jobsService.getAllCities(),
      this.jobsService.getWardByCityId('0'),
    ]).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private search(page: number = 1): void {
    this.pageIndex.set(page);
console.log(this.form.value);

    const req: JobListRequestModel = {
      ...this.form.value,
      pageIndex: this.pageIndex(),
      pageSize: this.pageSize(),
      isActive: true,
    };
    const subscription = this.jobsService.getAllJobs(req).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private fetchWardsByCityId(cityId: string): void {
    const subscription = this.jobsService.getWardByCityId(cityId).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
  
  private getOptionId(value: string, options: any[]): string {
      return options.find(
          option => option.name === value || 
          option.industryName === value || 
          option.categoryName === value
      )?.id || (options.some(option => option.industryName || option.categoryName) ? '0' : '');
  }

  private getPostedDateId(value: string): string {
      switch (value) {
          case this.multiLanguageService.getTranslatedLang('jobs.filter.postedDate.all-posted-date'):
              return '';
          case this.multiLanguageService.getTranslatedLang('jobs.filter.postedDate.today'):
              return '0';
          case this.multiLanguageService.getTranslatedLang('jobs.filter.postedDate.yesterday'):
              return '1';
          case this.multiLanguageService.getTranslatedLang('jobs.filter.postedDate.last-7-days'):
              return '2';
          case this.multiLanguageService.getTranslatedLang('jobs.filter.postedDate.last-30-days'):
              return '3';
          default:
              return '';
      }
  }
}