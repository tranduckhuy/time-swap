import { Component, computed, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { filter } from 'rxjs';

import { AuthService } from '../../auth/auth.service';
import { MultiLanguageService } from '../../../shared/services/multi-language.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, TranslateModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  private readonly authService = inject(AuthService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  // ? State Management
  currentLanguage = this.multiLanguageService.language;
  
  isHome = signal<boolean>(false);
  isLoggedIn = computed<boolean>(() => this.authService.isLoggedIn());

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

  onLogout() {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['./'], {
          relativeTo: this.activatedRoute,
          onSameUrlNavigation: 'reload',
        });
      }
    });
  }
}
