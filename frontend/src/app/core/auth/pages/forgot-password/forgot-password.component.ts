import { Component, DestroyRef, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { TranslateModule } from '@ngx-translate/core';

import { PreLoaderComponent } from '../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../shared/components/toast/toast.component';

import { getErrorMessage } from '../../../../shared/utils/form-validators';

import { AuthService } from '../../auth.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

import { ForgotPasswordRequestModel } from '../../../../shared/models/api/request/forgot-password-request.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    TranslateModule,
    ReactiveFormsModule,
    RouterLink,
    PreLoaderComponent,
    ToastComponent,
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css',
})
export class ForgotPasswordComponent {
  // ? Form Properties
  form!: FormGroup;

  // ? State Management
  isLoading = signal<boolean>(true);

  // ? Dependency Injection
  private readonly authService = inject(AuthService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

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

    const req: ForgotPasswordRequestModel = this.form.value;
    this.form.reset();
    const subscription = this.authService.forgotPassword(req).subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
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
      email: ['', [Validators.required, Validators.email]],
    });
  }
}
