import { Component } from '@angular/core';
import { BannerComponent } from '../../../../../shared/components/banner/banner.component';
import { BreadcrumbComponent } from '../../../../../shared/components/breadcrumb/breadcrumb.component';
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { TranslateModule } from '@ngx-translate/core';
import { ApplicantCardComponent } from './applicant-card/applicant-card.component';

@Component({
  selector: 'app-applicants',
  standalone: true,
  imports: [BannerComponent, BreadcrumbComponent, ApplicantCardComponent, PaginationComponent, TranslateModule],
  templateUrl: './applicants.component.html',
  styleUrl: './applicants.component.css'
})
export class ApplicantsComponent {

}
