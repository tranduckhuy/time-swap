import { Component, computed, DestroyRef, inject, input, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { GridLayoutComponent } from "../../../../../core/layout/grid-layout/grid-layout.component";
import { BannerDetailComponent } from "../../../../../shared/components/banner/banner-detail/banner-detail.component";
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { ButtonWithIconComponent } from "../../../../../shared/components/button-with-icon/button-with-icon.component";
import { JobPostComponent } from "../../../../../shared/components/job-post/job-post.component";
import { PreLoaderComponent } from "../../../../../shared/components/pre-loader/pre-loader.component";

import { CustomCurrencyPipe } from "../../../../../shared/pipes/custom-currency.pipe";

import { ENGLISH, VIETNAMESE } from '../../../../../shared/constants/multi-lang-constants';

import { MultiLanguageService } from '../../../../../shared/services/multi-language.service';

import type { JobDetailResponseModel } from '../../../../../shared/models/api/response/jobs-response.model';

@Component({
  selector: 'app-job-detail',
  standalone: true,
  imports: [
    TranslateModule,
    RouterLink,
    DatePipe,
    CustomCurrencyPipe,
    GridLayoutComponent,
    ButtonWithIconComponent,
    BannerDetailComponent,
    BannerComponent,
    JobPostComponent,
    PreLoaderComponent,
],
  templateUrl: './job-detail.component.html',
  styleUrl: './job-detail.component.css'
})
export class JobDetailComponent implements OnInit {
  // ? Data Resolver
  job = input.required<JobDetailResponseModel>();

  // ? Dependency Injection
  private multiLanguageService = inject(MultiLanguageService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  // ? State management
  isLoading = signal<boolean>(true);
  lang = computed(() => this.multiLanguageService.language() === VIETNAMESE ? VIETNAMESE : ENGLISH);

  ngOnInit(): void {
    if (!this.job()) {
      this.router.navigateByUrl('/not-found');
    }

    const timeOutId = setTimeout(() => this.isLoading.set(false), 800);

    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }
}
