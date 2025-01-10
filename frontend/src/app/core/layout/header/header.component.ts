import { Component, OnInit, DestroyRef, inject, signal, computed } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { filter } from 'rxjs';

import { CustomCurrencyPipe } from '../../../shared/pipes/custom-currency.pipe';

import { ENGLISH, VIETNAMESE } from '../../../shared/constants/multi-lang-constants';

import { AuthService } from '../../auth/auth.service';
import { ProfileService } from '../../../modules/user/pages/profile/profile.service';
import { MultiLanguageService } from '../../../shared/services/multi-language.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, TranslateModule, CustomCurrencyPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly profileService = inject(ProfileService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  // ? State Management
  currentLanguage = this.multiLanguageService.language;
  user = this.profileService.user;
  
  isHome = signal<boolean>(false);
  currentTheme = signal<string>(localStorage.getItem('theme') ?? 'theme-light');

  isLoggedIn = computed<boolean>(() => this.authService.isLoggedIn());
  lang = computed(() => this.multiLanguageService.language() === VIETNAMESE ? VIETNAMESE : ENGLISH);

  constructor() {
    // ? Subscribe to router events
    const routerSubscription = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const currentUrl = event.urlAfterRedirects;
        this.isHome.set(currentUrl === '/' || currentUrl.startsWith('/home'));
      });

    // ? Set up interval to check localStorage
    const intervalId = setInterval(() => {
      const theme = localStorage.getItem('theme');
      if (theme !== this.currentTheme()) {
        this.currentTheme.set(theme ?? 'theme-light');
      }
    }, 100);

    // ? Cleanup
    this.destroyRef.onDestroy(() => {
      clearInterval(intervalId);
      routerSubscription.unsubscribe();
    });
  }

  ngOnInit(): void {
    // ? Get user profile if logged in
    if (this.isLoggedIn()) {
      const profileSubscription = this.profileService.getUserProfile().subscribe();
      this.destroyRef.onDestroy(() => profileSubscription.unsubscribe());
    }
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

  shouldShowLogoOne() {
    return this.isHome() && this.currentTheme() === 'theme-light';
  }
}
