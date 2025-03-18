import {
  Component,
  computed,
  DestroyRef,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';

import { forkJoin, of, switchMap } from 'rxjs';

import { TranslateModule } from '@ngx-translate/core';

import { NiceSelectComponent } from '../../../../../../shared/components/nice-select/nice-select.component';
import { PreLoaderComponent } from '../../../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../../../shared/components/toast/toast.component';

import { ProfileService } from '../../profile.service';
import { LocationService } from '../../../../../../shared/services/location.service';
import { IndustryService } from '../../../../../../shared/services/industry.service';
import { CategoryService } from '../../../../../../shared/services/category.service';

@Component({
  selector: 'app-my-profile',
  standalone: true,
  imports: [
    TranslateModule,
    DatePipe,
    ReactiveFormsModule,
    NiceSelectComponent,
    PreLoaderComponent,
    ToastComponent,
  ],
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css'],
})
export class MyProfileComponent implements OnInit {
  // Dependency Injection
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  private readonly profileService = inject(ProfileService);
  private readonly locationService = inject(LocationService);
  private readonly industryService = inject(IndustryService);
  private readonly categoryService = inject(CategoryService);

  // Form Properties
  form!: FormGroup;

  // State Management
  user = this.profileService.user;
  subscription = this.profileService.subscription;
  industries = this.industryService.industries;
  categories = this.categoryService.categories;
  cities = this.locationService.cities;
  wards = this.locationService.wards;
  isLoading = this.profileService.isLoading;
  isEditing = signal(false);

  // Computed Properties
  citiesName = computed(() => this.cities().map((c) => c.name));
  wardsName = computed(() => this.wards().map((w) => w.fullLocation));
  industriesName = computed(() => this.industries().map((i) => i.industryName));
  categoriesName = computed(() => this.categories().map((i) => i.categoryName));

  sortedIndustries = computed(() => {
    const allIndustries = this.industries();
    const userIndustry = this.user()?.majorIndustry?.id;
    return this.prioritizeSelected(allIndustries, userIndustry).map(
      (i) => i.industryName,
    );
  });

  sortedCities = computed(() => {
    const allCities = this.cities();
    const userCity = this.user()?.city.id;
    return this.prioritizeSelected(allCities, userCity).map((c) => c.name);
  });

  sortedWards = computed(() => {
    const allWards = this.wards();
    const userWard = this.user()?.ward?.id;
    return this.prioritizeSelected(allWards, userWard).map(
      (w) => w.fullLocation,
    );
  });

  ngOnInit(): void {
    this.initializeForm();
    this.profileService.getUserProfile().subscribe();
  }

  private initializeForm(): void {
    this.form = this.fb.group({
      fullName: [{ value: this.user()?.fullName || '', disabled: true }],
      phoneNumber: [{ value: this.user()?.phoneNumber || '', disabled: true }],
      fullLocation: [
        { value: this.user()?.ward.fullLocation || '', disabled: true },
      ],
      cityId: [{ value: this.user()?.city.id, disabled: true }],
      wardId: [{ value: this.user()?.ward.id, disabled: true }],
      majorIndustryId: [
        {
          value: this.user()?.majorIndustry.industryName || '',
          disabled: true,
        },
      ],
      majorCategoryId: [
        {
          value: this.user()?.majorCategory.categoryName || '',
          disabled: true,
        },
      ],
      description: [{ value: this.user()?.description || '', disabled: true }],
    });
  }

  onSubmit(): void {
    if (this.isEditing()) {
      this.toggleEditing(false);
      const user = this.user();
      if (user) {
        this.profileService
          .updateUserProfile(
            this.form.value,
            this.industries(),
            this.categories(),
            this.cities(),
            this.wards(),
            user,
          )
          .subscribe();
      }
    } else {
      this.toggleEditing(true);
      this.fetchInitialData();
    }
  }

  handleSelectChange(field: string, value: string, options: any[]): void {
    const id = this.getOptionId(value, options);

    const formattedId =
      field === 'majorIndustryId' || field === 'majorCategoryId'
        ? Number(id) || null
        : id;

    this.form.get(field)?.setValue(formattedId);

    if (field === 'majorIndustryId' && id) {
      this.loadCategories(formattedId as number);
    }

    if (field === 'cityId' && id) {
      this.fetchWardsByCityId(formattedId as string);
    }
  }

  private fetchInitialData(): void {
    const subscription = this.industryService
      .getAllIndustries()
      .pipe(
        switchMap(() => {
          const industryId = this.user()?.majorIndustry.id;
          const categoryObservable = industryId
            ? this.categoryService.getCategoriesByIndustryId(industryId)
            : of(void 0);

          return forkJoin([
            this.locationService.getAllCities(),
            this.locationService.getWardByCityId(this.user()?.city.id ?? '0'),
            categoryObservable,
          ]);
        }),
      )
      .subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private fetchWardsByCityId(cityId: string): void {
    const subscription = this.locationService
      .getWardByCityId(cityId)
      .subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private loadCategories(industryId: number): void {
    if (industryId === undefined) return;

    const subscription = this.categoryService
      .getCategoriesByIndustryId(industryId)
      .subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private getOptionId(value: string, options: any[]): string | number {
    return (
      options.find((option) =>
        [
          option.name,
          option.industryName,
          option.categoryName,
          option.fullLocation,
        ].includes(value),
      )?.id || ''
    );
  }

  private prioritizeSelected<T extends { id: any }>(
    list: T[],
    selectedId: any,
  ): T[] {
    if (!selectedId) return list;
    return [...list].sort((a, b) =>
      a.id === selectedId ? -1 : b.id === selectedId ? 1 : 0,
    );
  }

  private toggleEditing(state: boolean): void {
    this.isEditing.set(state);
    state ? this.form.enable() : this.form.disable();
  }
}
