import { Component, DestroyRef, inject, signal } from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';
import { filter } from 'rxjs';

import { MultiLanguageService } from '../../../shared/services/multi-language.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, TranslateModule],
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  isHome = signal<boolean>(false);
  
  private readonly multiLanguageService = inject(MultiLanguageService);
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

  onChangeLanguage(lang: string) {
    this.multiLanguageService.updateLanguage(lang);
  }
}
