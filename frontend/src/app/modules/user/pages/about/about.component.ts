import { Component, OnInit, DestroyRef, inject, signal } from '@angular/core';

import { BannerComponent } from '../../../../shared/components/banner/banner.component';
import { BreadcrumbComponent } from "../../../../shared/components/breadcrumb/breadcrumb.component";
import { PreLoaderComponent } from "../../../../shared/components/pre-loader/pre-loader.component";

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [BannerComponent, BreadcrumbComponent, PreLoaderComponent],
  templateUrl: './about.component.html',
  styleUrl: './about.component.css'
})
export class AboutComponent implements OnInit {
  isLoading = signal<boolean>(true);

  private destroyRef = inject(DestroyRef)

  ngOnInit(): void {
    const timeOutId = setTimeout(() => {
      this.isLoading.set(false);
    }, 600);

    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }
}
