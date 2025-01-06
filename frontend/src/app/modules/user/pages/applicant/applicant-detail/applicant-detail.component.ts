import { Component } from '@angular/core';
import { GridLayoutComponent } from '../../../../../core/layout/grid-layout/grid-layout.component';
import { ButtonWithIconComponent } from '../../../../../shared/components/button-with-icon/button-with-icon.component';
import { TranslateModule } from '@ngx-translate/core';
import { BannerComponent } from '../../../../../shared/components/banner/banner.component';
import { BannerDetailComponent } from '../../../../../shared/components/banner/banner-detail/banner-detail.component';

@Component({
  selector: 'app-applicant-detail',
  standalone: true,
  imports: [BannerComponent, BannerDetailComponent, GridLayoutComponent, ButtonWithIconComponent, TranslateModule],
  templateUrl: './applicant-detail.component.html',
  styleUrl: './applicant-detail.component.css'
})
export class ApplicantDetailComponent {

}
