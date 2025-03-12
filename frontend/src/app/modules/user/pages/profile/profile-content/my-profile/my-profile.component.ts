import {
  Component,
  computed,
  DestroyRef,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { DatePipe } from '@angular/common';

import { forkJoin } from 'rxjs';

import { TranslateModule } from '@ngx-translate/core';

import { NiceSelectComponent } from '../../../../../../shared/components/nice-select/nice-select.component';
import { PreLoaderComponent } from '../../../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../../../shared/components/toast/toast.component';

import { ProfileService } from '../../profile.service';
import { ToastHandlingService } from '../../../../../../shared/services/toast-handling.service';
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
  private readonly profileService = inject(ProfileService);
  private readonly locationService = inject(LocationService);
  private readonly industryService = inject(IndustryService);
  private readonly categoryService = inject(CategoryService)
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  private readonly toastHandlingService = inject(ToastHandlingService);

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

  ngOnInit(): void {
    this.initializeForm();
    this.profileService.getUserProfile().subscribe();
  }

  private initializeForm(): void {
    this.form = this.fb.group({
      fullName: [{ value: this.user()?.fullName || '', disabled: true }],
      phoneNumber: [{ value: this.user()?.phoneNumber || '', disabled: true }],
      fullLocation: [{ value: this.user()?.fullLocation || '', disabled: true }],
      cityId: [{ value: '', disabled: true }],
      wardId: [{ value: '', disabled: true }],
      majorIndustryId: [{
        value: this.user()?.majorIndustry || '',
        disabled: true,
      }],
      description: [{ value: this.user()?.description || '', disabled: true }],
    });
  }

  onSubmit(): void {
    if (this.isEditing()) {
      this.toggleEditing(false);
      const user = this.user();
      if (user) {
        this.profileService
        .updateUserProfile(this.form.value, this.industries(), this.categories(), this.wards(), user)
        .subscribe();
      }
    } else {
      this.toggleEditing(true);
      this.fetchInitialData();
    }
  }

  handleSelectChange(field: string, value: string, options: any[]): void {
    const id = this.getOptionId(value, options);
    this.form.get(field)?.setValue(id);

    if (field === 'cityId' && id) {
      this.fetchWardsByCityId(id);
    }
  }

  private fetchInitialData(): void {
    const subscription = forkJoin([
      this.locationService.getAllCities(),
      this.locationService.getWardByCityId('0'),
      this.industryService.getAllIndustries(),
    ]).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private fetchWardsByCityId(cityId: string): void {
    const subscription = this.locationService
      .getWardByCityId(cityId)
      .subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private getOptionId(value: string, options: any[]): string {
    return (
      options.find(
        (option) =>
          [option.name, option.industryName, option.categoryName, option.fullLocation].includes(value)
      )?.id || ''
    );
  }

  private toggleEditing(state: boolean): void {
    this.isEditing.set(state);
    state ? this.form.enable() : this.form.disable();
  }

  private showSuccessToast(): void {
    this.toastHandlingService.handleSuccess('profile.notify.update-success');
  }

  private showErrorToast(): void {
    this.toastHandlingService.handleError('profile.notify.update-fail');
  }
}
