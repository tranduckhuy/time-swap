import { Component, computed, DestroyRef, effect, inject, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { filter } from 'rxjs';

import { AuthService } from '../../auth/auth.service';
import { ProfileService } from '../../../modules/user/pages/profile/profile.service';
import { MultiLanguageService } from '../../../shared/services/multi-language.service';

type UserInfo = {
  name: string,
  avatarUrl: string
}

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, TranslateModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  private readonly authService = inject(AuthService);
  private readonly profileService = inject(ProfileService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  // ? State Management
  currentLanguage = this.multiLanguageService.language;
  
  isHome = signal<boolean>(false);
  currentTheme = signal<string>(localStorage.getItem('theme') ?? 'theme-light');

  isLoggedIn = computed<boolean>(() => this.authService.isLoggedIn());
  userInfo = computed<UserInfo | null>(() => {
    const user = this.profileService.user();
    if (user) {
      return {
        name: user.fullName ?? '',
        avatarUrl: user.avatarUrl ?? ''
      };
    }
    return null;
  });

  constructor() {
    // ? Subscribe to router events
    const subscription = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const currentUrl = event.urlAfterRedirects;
        this.isHome.set(currentUrl === '/' || currentUrl.startsWith('/home'));
      });

    // ? Effect: Check theme changes
    effect(() => {
      const checkTheme = () => {
        const theme = localStorage.getItem('theme');
        if (theme !== this.currentTheme()) {
          this.currentTheme.set(theme ?? 'theme-light');
        }
      };

      // ? Initial check
      checkTheme();

      // ? Set up interval to check localStorage
      const intervalId = setInterval(checkTheme, 100);

      // ? Cleanup
      this.destroyRef.onDestroy(() => {
        clearInterval(intervalId);
        subscription.unsubscribe();
      });

      // ? Get user profile if logged in
      if (this.isLoggedIn()) {
        this.profileService.getUserProfile().subscribe();
      }
    });
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
