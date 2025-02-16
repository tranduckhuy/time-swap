import { Component, inject } from '@angular/core';
import {
  ActivatedRoute,
  Router,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { AuthService } from '../../../../../core/auth/auth.service';

@Component({
  selector: 'app-profile-tabs',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, TranslateModule],
  templateUrl: './profile-tabs.component.html',
  styleUrl: './profile-tabs.component.css',
})
export class ProfileTabsComponent {
  private readonly authService = inject(AuthService);

  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly router = inject(Router);

  onLogout() {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/home'], {
          relativeTo: this.activatedRoute,
          onSameUrlNavigation: 'reload',
        });
      },
    });
  }
}
