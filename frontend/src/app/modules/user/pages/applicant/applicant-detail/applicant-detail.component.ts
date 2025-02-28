import { Component, inject, input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { GridLayoutComponent } from '../../../../../core/layout/grid-layout/grid-layout.component';
import { ButtonWithIconComponent } from '../../../../../shared/components/button-with-icon/button-with-icon.component';
import { BannerComponent } from '../../../../../shared/components/banner/banner.component';
import { BannerDetailComponent } from '../../../../../shared/components/banner/banner-detail/banner-detail.component';

import { UserModel } from '../../../../../shared/models/entities/user.model';

@Component({
  selector: 'app-applicant-detail',
  standalone: true,
  imports: [
    BannerComponent,
    BannerDetailComponent,
    GridLayoutComponent,
    ButtonWithIconComponent,
    TranslateModule,
  ],
  templateUrl: './applicant-detail.component.html',
  styleUrl: './applicant-detail.component.css',
})
export class ApplicantDetailComponent implements OnInit {
  user = input.required<UserModel>();

  router = inject(Router);

  ngOnInit(): void {
    if (!this.user) {
      this.router.navigateByUrl('/not-found');
    }
  }
}
