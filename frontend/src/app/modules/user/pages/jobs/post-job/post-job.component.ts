import { Component } from '@angular/core';
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { BreadcrumbComponent } from "../../../../../shared/components/breadcrumb/breadcrumb.component";

@Component({
  selector: 'app-post-job',
  standalone: true,
  imports: [BannerComponent, BreadcrumbComponent],
  templateUrl: './post-job.component.html',
  styleUrl: './post-job.component.css'
})
export class PostJobComponent {

}
