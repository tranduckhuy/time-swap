import { Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { TranslateModule } from '@ngx-translate/core';

import { PreLoaderComponent } from '../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../shared/components/toast/toast.component';

import {
  controlValueEqual,
  getErrorMessage,
} from '../../../../shared/utils/form-validators';

import { AuthService } from '../../auth.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

import type { ResetPasswordRequestModel } from '../../../../shared/models/api/request/reset-password-request.model';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    TranslateModule,
    ReactiveFormsModule,
    RouterLink,
    PreLoaderComponent,
    ToastComponent,
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent {
  // ? Form Properties
  form!: FormGroup;

  // ? State Management
  isLoading = signal<boolean>(true);

  // ? Dependency Injection
  private readonly authService = inject(AuthService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  private readonly activatedRoute = inject(ActivatedRoute);

  ngOnInit(): void {
    this.initialForm();

    const timeOutId = setTimeout(() => this.isLoading.set(false), 800);

    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.activatedRoute.queryParams.subscribe((params) => {
      const token = params['token'] || '';
      const email = params['email'] || '';

      const req: ResetPasswordRequestModel = {
        ...this.form.value,
        token,
        email,
      };

      const subscription = this.authService.resetPassword(req).subscribe();
      this.destroyRef.onDestroy(() => subscription.unsubscribe());
    });
  }

  isControlInvalid(controlName: string): boolean {
    const control = this.form.controls[controlName];
    return control?.invalid && control?.touched;
  }

  getMessage(controlName: string, nameKey: string) {
    return getErrorMessage(
      controlName,
      nameKey,
      this.form,
      this.multiLanguageService,
    );
  }

  private initialForm() {
    this.form = this.fb.group({
      password: ['', Validators.required, Validators.minLength(6)],
      confirmPassword: [
        '',
        [Validators.required, controlValueEqual('password', 'confirmPassword')],
      ],
    });
  }
}
