import { Component } from '@angular/core';
import { BannerComponent } from "../../../../shared/components/banner/banner.component";
import { BreadcrumbComponent } from "../../../../shared/components/breadcrumb/breadcrumb.component";

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [BannerComponent, BreadcrumbComponent],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent {

}