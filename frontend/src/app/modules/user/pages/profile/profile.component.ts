import { Component } from '@angular/core';
import { BannerComponent } from "../../../../shared/components/banner/banner.component";
import { BreadcrumbComponent } from "../../../../shared/components/breadcrumb/breadcrumb.component";
import { ProfileTabsComponent } from "./profile-tabs/profile-tabs.component";
import { ProfileContentComponent } from "./profile-content/profile-content.component";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [BannerComponent, BreadcrumbComponent, ProfileTabsComponent, ProfileContentComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {

}
