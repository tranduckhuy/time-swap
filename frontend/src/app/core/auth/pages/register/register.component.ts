import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { PreLoaderComponent } from '../../../../shared/components/pre-loader/pre-loader.component';

import { controlValueEqual, getErrorMessage } from '../../../../shared/utils/form-validators';

import { AUTH_CLIENT_URL } from '../../../../shared/constants/auth-constants';
import { REGISTER_SUCCESS_CODE } from '../../../../shared/constants/status-code-constants';
import { ERROR_MESSAGE, SUCCESS_MESSAGE } from '../../../../shared/constants/message-constants';

import type { RegisterRequestModel } from '../../../../shared/models/api/request/register-request.model';

import { AuthService } from '../../auth.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterLink, PreLoaderComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;
  isLoading = signal<boolean>(true);

  private readonly authService = inject(AuthService)
  private readonly toastService = inject(ToastService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly fb = inject(FormBuilder);

  ngOnInit(): void {
    this.initForm();

    const timeOutId = setTimeout(() => this.isLoading.set(false), 1000);
    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }

  initForm() {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/(84|0[35789])+(\d{8})\b/g)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, controlValueEqual('password', 'confirmPassword')]] 
    });
  }
  
  isControlInvalid(controlName: string): boolean {
    const control = this.form.controls[controlName];
    return control?.invalid && control?.touched;
  }

  getMessage(controlName: string, name: string) {
    return getErrorMessage(controlName, name, this.form, this.multiLanguageService);
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const req: RegisterRequestModel = {
      ...this.form.value,
      clientUrl: AUTH_CLIENT_URL
      
    };
    const subscription = this.authService.register(req).subscribe({
      next: (res) => {
        if (res.statusCode === REGISTER_SUCCESS_CODE) {
          this.toastService.success(SUCCESS_MESSAGE, this.multiLanguageService.getTranslatedLang('auth.register.success'));
        }
      },
      error: () => this.toastService.error(ERROR_MESSAGE, this.multiLanguageService.getTranslatedLang('common.messages.error'))
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
