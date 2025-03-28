import { Component, DestroyRef, OnInit, computed, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { forkJoin } from 'rxjs';

import { TranslateModule } from '@ngx-translate/core';

import { BannerComponent } from '../../../../../shared/components/banner/banner.component';
import { BreadcrumbComponent } from '../../../../../shared/components/breadcrumb/breadcrumb.component';
import { NiceSelectComponent } from '../../../../../shared/components/nice-select/nice-select.component';
import { PreLoaderComponent } from '../../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../../shared/components/toast/toast.component';

import { ThousandPipe } from '../../../../../shared/pipes/thousand.pipe';

import { handleSelectChange } from '../../../../../shared/utils/util-functions';

import {
  ENGLISH,
  VIETNAMESE,
} from '../../../../../shared/constants/multi-lang-constants';

import { getErrorMessage } from '../../../../../shared/utils/form-validators';

import { JobsService } from '../jobs.service';
import { CategoryService } from '../../../../../shared/services/category.service';
import { IndustryService } from '../../../../../shared/services/industry.service';
import { LocationService } from '../../../../../shared/services/location.service';
import { FilterService } from '../../../../../shared/services/filter.service';
import { MultiLanguageService } from '../../../../../shared/services/multi-language.service';

import type { PostJobRequestModel } from '../../../../../shared/models/api/request/post-job-request.model';

@Component({
  selector: 'app-post-job',
  standalone: true,
  imports: [
    TranslateModule,
    ReactiveFormsModule,
    ThousandPipe,
    BannerComponent,
    BreadcrumbComponent,
    NiceSelectComponent,
    PreLoaderComponent,
    ToastComponent,
  ],
  templateUrl: './post-job.component.html',
  styleUrl: './post-job.component.css',
})
export class PostJobComponent implements OnInit {
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

  // ? State management
  isLoading = this.jobsService.isLoading;

  // ? Data For Select Options
  industries = this.industryService.industries;
  categories = this.categoryService.categories;
  cities = this.locationService.cities;
  wards = this.locationService.wards;

  // ? Computed Properties
  lang = computed(() =>
    this.multiLanguageService.language() === VIETNAMESE ? VIETNAMESE : ENGLISH,
  );
  industriesName = computed(() => this.industries().map((i) => i.industryName));
  citiesName = computed(() => this.cities().map((c) => c.name));
  categoriesName = computed(() => this.categories().map((c) => c.categoryName));
  wardsName = computed(() => this.wards().map((w) => w.fullLocation));

  ngOnInit(): void {
    this.initialForm();
    this.initialSelectOptions();
  }

  initialForm() {
    this.form = this.fb.group({
      title: ['', Validators.required],
      fee: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      responsibilities: [
        '',
        [Validators.required, Validators.pattern(/^[^,]+(,[^,]+)*$/)],
      ],
      description: [''],
      industryId: ['', Validators.required],
      categoryId: ['', Validators.required],
      cityId: ['', Validators.required],
      wardId: ['', Validators.required],
      startDate: ['', Validators.required],
      dueDate: ['', Validators.required],
    });
  }

  onSelectChange(field: string, value: string, options: any[]): void {
    handleSelectChange(
      field,
      value,
      options,
      this.form,
      this.filterService,
      this.categoryService,
      this.locationService,
    );
  }

  onSubmit() {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach((key) => {
        const control = this.form.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
      return;
    }

    const req: PostJobRequestModel = {
      ...this.form.value,
      fee: +this.form.get('fee')?.value,
      startDate: new Date(this.form.get('startDate')?.value).toISOString(),
      dueDate: new Date(this.form.get('dueDate')?.value).toISOString(),
    };

    const subscription = this.jobsService.createJob(req).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onInputFee(fee: string) {
    const value = fee.replace(/\D/g, '');

    if (value !== this.form.get('fee')?.value) {
      this.form.patchValue({ fee: value });
    }
  }

  isControlInvalid(controlName: string): boolean {
    const control = this.form.controls[controlName];
    return control?.invalid && control?.touched;
  }

  getMessage(controlName: string, nameKey: string) {
    return getErrorMessage(
      controlName,
      nameKey,
      this.form,
      this.multiLanguageService,
    );
  }

  private initialSelectOptions(): void {
    const subscription = forkJoin([
      this.industryService.getAllIndustries(),
      this.locationService.getAllCities(),
    ]).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
