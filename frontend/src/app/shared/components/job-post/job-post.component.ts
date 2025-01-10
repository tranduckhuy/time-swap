import { Component, computed, inject, input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';

import { CustomCurrencyPipe } from '../../pipes/custom-currency.pipe';

import { ENGLISH, VIETNAMESE } from '../../constants/multi-lang-constants';

import { MultiLanguageService } from '../../services/multi-language.service';

import type { JobPostModel } from '../../models/entities/job.model';

@Component({
  selector: 'app-job-post',
  standalone: true,
  imports: [DatePipe, CustomCurrencyPipe],
  templateUrl: './job-post.component.html',
  styleUrl: './job-post.component.css'
})
export class JobPostComponent {
  private readonly router = inject(Router);
  private readonly multiLanguageService = inject(MultiLanguageService);

  job = input.required<JobPostModel>();
  lang = computed(() => this.multiLanguageService.language() === VIETNAMESE ? VIETNAMESE : ENGLISH);

  navigateToJobPost(jobId: string) {
    this.router.navigate(['/jobs', jobId]).then(() => {
      window.scroll({
        top: 0,
        behavior: 'smooth'
      });
    });
  }
}
