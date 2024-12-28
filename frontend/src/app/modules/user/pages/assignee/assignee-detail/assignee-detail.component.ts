import { Component } from '@angular/core';
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { BannerDetailComponent } from "../../../../../shared/components/banner/banner-detail/banner-detail.component";
import { GridLayoutComponent } from "../../../../../core/layout/grid-layout/grid-layout.component";
import { ButtonWithIconComponent } from "../../../../../shared/components/button-with-icon/button-with-icon.component";

@Component({
  selector: 'app-assignee-detail',
  standalone: true,
  imports: [BannerComponent, BannerDetailComponent, GridLayoutComponent, ButtonWithIconComponent],
  templateUrl: './assignee-detail.component.html',
  styleUrl: './assignee-detail.component.css'
})
export class AssigneeDetailComponent {

}
