import { DatePipe } from '@angular/common';
import { Component, inject, input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { GridLayoutComponent } from "../../../../../core/layout/grid-layout/grid-layout.component";
import { BannerDetailComponent } from "../../../../../shared/components/banner/banner-detail/banner-detail.component";
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { ButtonWithIconComponent } from "../../../../../shared/components/button-with-icon/button-with-icon.component";
import { JobAlertComponent } from "../../../../../shared/components/job-alert/job-alert.component";
import { JobPostComponent } from "../../../../../shared/components/job-post/job-post.component";

import type { JobListRequestModel } from '../../../../../shared/models/api/request/job-list-request.model';
import type { JobDetailResponseModel } from '../../../../../shared/models/api/response/jobs-response.model';

@Component({
  selector: 'app-job-detail',
  standalone: true,
  imports: [
    TranslateModule,
    DatePipe,
    GridLayoutComponent,
    JobAlertComponent,
    ButtonWithIconComponent,
    BannerDetailComponent,
    BannerComponent,
    JobPostComponent
],
  templateUrl: './job-detail.component.html',
  styleUrl: './job-detail.component.css'
})
export class JobDetailComponent implements OnInit {
  // ? Input Properties got from parent component
  queryParams = input<JobListRequestModel>();

  // ? Data Resolver
  job = input.required<JobDetailResponseModel>();

  // ? Dependency Injection
  private router = inject(Router);

  ngOnInit(): void {
    if (!this.job()) {
      this.router.navigateByUrl('/not-found');
    }
  }
}
