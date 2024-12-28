import { Component, DestroyRef, inject, signal } from '@angular/core';

import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  isHome = signal<boolean>(false);
  
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  constructor() {
    const subscription = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const currentUrl = event.urlAfterRedirects;
        if (currentUrl === '/' || currentUrl === '/home' || currentUrl.includes('/home')) {
          this.isHome.set(true);
        } else {
          this.isHome.set(false);
        }
      });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
