import { Component } from '@angular/core';
import { BannerComponent } from "../../../../../shared/components/banner/banner.component";
import { BreadcrumbComponent } from "../../../../../shared/components/breadcrumb/breadcrumb.component";
import { AssigneeCardComponent } from "./assignee-card/assignee-card.component";
import { PaginationComponent } from '../../../../../shared/components/pagination/pagination.component';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-assignees',
  standalone: true,
  imports: [BannerComponent, BreadcrumbComponent, AssigneeCardComponent, PaginationComponent, TranslateModule],
  templateUrl: './assignees.component.html',
  styleUrl: './assignees.component.css'
})
export class AssigneesComponent {

}
