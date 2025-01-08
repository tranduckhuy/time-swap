import { Component } from '@angular/core';

import { TranslateModule } from '@ngx-translate/core';

import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { BannerDetailComponent } from "../../../../../shared/components/banner/banner-detail/banner-detail.component";
import { GridLayoutComponent } from "../../../../../core/layout/grid-layout/grid-layout.component";
import { ButtonWithIconComponent } from "../../../../../shared/components/button-with-icon/button-with-icon.component";
import { ReviewCardComponent } from "../../../../../shared/components/review-card/review-card.component";
import { ReviewFormComponent } from "../../../../../shared/components/review-form/review-form.component";

@Component({
  selector: 'app-assignee-detail',
  standalone: true,
  imports: [
    TranslateModule, 
    BannerComponent, 
    BannerDetailComponent, 
    GridLayoutComponent, 
    ButtonWithIconComponent, 
    ReviewCardComponent, 
    ReviewFormComponent
  ],
  templateUrl: './assignee-detail.component.html',
  styleUrl: './assignee-detail.component.css'
})
export class AssigneeDetailComponent {

}
