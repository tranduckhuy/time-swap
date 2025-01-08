import { Component, computed, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ProfileService } from '../../profile.service';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NiceSelectComponent } from "../../../../../../shared/components/nice-select/nice-select.component";
import { JobsService } from '../../../jobs/jobs.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-my-profile',
  standalone: true,
  imports: [TranslateModule, DatePipe, ReactiveFormsModule, NiceSelectComponent],
  templateUrl: './my-profile.component.html',
  styleUrl: './my-profile.component.css'
})
export class MyProfileComponent implements OnInit {
  // ? Dependency Injection
  private readonly profileService = inject(ProfileService);
  private readonly jobsService = inject(JobsService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  // ? Form Properties
  form!: FormGroup;

  // ? State Management
  user = this.profileService.user;
  subscription = this.profileService.subscription;  
  cities = this.jobsService.cities;
  wards = this.jobsService.wards;
  isEditing = signal<boolean>(false);

  // ? Computed Properties
  citiesName = computed(() => this.cities().map(c => c.name));
  wardsName = computed(() => this.wards().map(w => w.fullLocation));

  ngOnInit(): void {
    this.form = this.fb.group({
      fullName: [{ value: this.user()?.fullName || '', disabled: true }],
      phoneNumber: [{ value: this.user()?.phoneNumber || '', disabled: true }],
      fullLocation: [{ value: this.user()?.fullLocation || '', disabled: true }],
      cityId: '',
      wardId: '',
      // job: [{ value: '', disabled: true }],
      // avatar: [{ value: '', disabled: true }]
    });

    this.profileService.getUserProfile().subscribe();
  }

  onSubmit() {
    if (this.isEditing()) {
      this.isEditing.set(false);
      this.form.disable();
      console.log(this.form.value);
      if (this.form.valid) {
        
        // this.profileService.updateUserProfile(this.form.value).subscribe(() => {
          // });
        }
      } else {
        this.isEditing.set(true);
        this.form.enable();
        const subscription = forkJoin([
          this.jobsService.getAllCities(),
          this.jobsService.getWardByCityId('0'),
        ]).subscribe();
    
        this.destroyRef.onDestroy(() => subscription.unsubscribe());
    }
  }

  handleSelectChange(field: string, value: string, options: any[]): void {
    const id = this.getOptionId(value, options);
    
    this.form.get(field)?.setValue(id);

    if (field === 'cityId' && id) {
        this.fetchWardsByCityId(id);
    }
  } 

  private fetchWardsByCityId(cityId: string): void {
    const subscription = this.jobsService.getWardByCityId(cityId).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
  private getOptionId(value: string, options: any[]): string {
    return options.find(option => option.name === value)?.id;
}
}
