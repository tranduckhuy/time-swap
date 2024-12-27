import { Component } from '@angular/core';
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { BannerDetailComponent } from "../../../../../shared/components/banner/banner-detail/banner-detail.component";

@Component({
  selector: 'app-assignee-detail',
  standalone: true,
  imports: [BannerComponent, BannerDetailComponent],
  templateUrl: './assignee-detail.component.html',
  styleUrl: './assignee-detail.component.css'
})
export class AssigneeDetailComponent {

}
