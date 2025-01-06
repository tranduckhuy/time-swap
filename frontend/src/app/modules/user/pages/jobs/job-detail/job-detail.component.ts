import { DatePipe } from '@angular/common';
import { Component, inject, input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { DUMMY_JOBS } from '../dummy-job';

import { GridLayoutComponent } from "../../../../../core/layout/grid-layout/grid-layout.component";
import { BannerDetailComponent } from "../../../../../shared/components/banner/banner-detail/banner-detail.component";
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { ButtonWithIconComponent } from "../../../../../shared/components/button-with-icon/button-with-icon.component";
import { JobAlertComponent } from "../../../../../shared/components/job-alert/job-alert.component";

import { JobListRequestModel } from '../../../../../shared/models/api/request/job-list-request.model';
import { JobPostModel } from '../../../../../shared/models/entities/job.model';

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
    BannerComponent
  ],
  templateUrl: './job-detail.component.html',
  styleUrl: './job-detail.component.css'
})
export class JobDetailComponent implements OnInit {
  relatedJobs = DUMMY_JOBS.slice(0, 3);

  // ? Input Properties got from parent component
  queryParams = input<JobListRequestModel>();

  // ? Data Resolver
  job = input.required<JobPostModel>();

  // ? Dependency Injection
  private router = inject(Router);

  ngOnInit(): void {
    if (!this.job()) {
      this.router.navigateByUrl('/error');
    }
  }
}
