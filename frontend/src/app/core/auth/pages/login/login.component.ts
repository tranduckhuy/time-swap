import { Component, DestroyRef, inject, input, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { ToastComponent } from "../../../../shared/components/toast/toast.component";
import { PreLoaderComponent } from "../../../../shared/components/pre-loader/pre-loader.component";

import { getErrorMessage } from '../../../../shared/utils/form-validators';
import { showToast } from '../../../../shared/utils/util-functions';

import { AUTH_CLIENT_URL } from '../../../../shared/constants/auth-constants';
import { 
  NOT_CONFIRM_CODE, 
  REGISTER_CONFIRM_SUCCESS_CODE, 
  SUCCESS_CODE, 
  TOKEN_EXPIRED_CODE 
} from '../../../../shared/constants/status-code-constants';

import type { LoginRequestModel } from '../../../../shared/models/api/request/login-request.model';
import type { ReConfirmRequestModel } from '../../../../shared/models/api/request/confirm-request.model';

import { AuthService } from '../../auth.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterLink, ToastComponent, PreLoaderComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  userId = signal<string>('');
  isLoading = signal<boolean>(true);
  token = input<string>('');
  email = input<string>('');

  private readonly authService = inject(AuthService);
  private readonly toastService = inject(ToastService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  ngOnInit(): void {
    if (this.token() && this.email()) {
      this.confirmEmail();
    }

    this.initForm();

    const timeOutId = setTimeout(() => this.isLoading.set(false), 1000);
    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }

  initForm() {
    this.form = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
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
    
    const req: LoginRequestModel = this.form.value;
    const subscription = this.authService.signin(req)
      .subscribe({
        next: (res) => {
          if (res.data) {
            const { accessToken, refreshToken, expiresIn } = res.data;
            this.form.reset();
            this.authService.saveLocalData(accessToken, refreshToken, expiresIn);
            this.router.navigateByUrl('/home');
          } else if (res.statusCode === NOT_CONFIRM_CODE) {
            showToast(
              this.toastService, 
              this.multiLanguageService, 
              'warn', 
              'common.notify.warning-title', 
              'auth.login.not-confirm'
            );
          } else {
            showToast(
              this.toastService, 
              this.multiLanguageService, 
              'error', 
              'common.notify.error-title', 
              'common.notify.error-message'
            );
          }
        },
        error: () => 
          showToast(
            this.toastService, 
            this.multiLanguageService, 
            'error', 
            'common.notify.error-title', 
            'common.notify.error-message'
          )
      });
      
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  confirmEmail() {
    const decodedToken = decodeURIComponent(this.token());
    const req = {
      token: decodedToken,
      email: this.email()
    }
    const subscription = this.authService.confirmEmail(req).subscribe({
      next: (res) => {
        if (res.statusCode === SUCCESS_CODE) {
          showToast(
            this.toastService, 
            this.multiLanguageService, 
            'success', 
            'common.notify.success-title', 
            'auth.login.confirm-success'
          );
        } else {
          showToast(
            this.toastService, 
            this.multiLanguageService, 
            'error', 
            'common.notify.error-title', 
            'common.notify.error-message'
          );
        }
      },
      error: (error: HttpErrorResponse) => {
        if (error.error.statusCode === TOKEN_EXPIRED_CODE) {
          showToast(
            this.toastService, 
            this.multiLanguageService, 
            'warn', 
            'common.notify.warning-title', 
            'auth.login.token-expired'
          );
          this.reConfirmEmail();
        } else {
          showToast(
            this.toastService, 
            this.multiLanguageService, 
            'error', 
            'common.notify.error-title', 
            'common.notify.error-message'
          );
        }
      } 
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  reConfirmEmail() {
    const req: ReConfirmRequestModel = {
      email: this.email(),
      clientUrl: AUTH_CLIENT_URL
    }
    const subscription = this.authService.resendConfirmEmail(req).subscribe({
      next: (res) => {
        if (res.statusCode === REGISTER_CONFIRM_SUCCESS_CODE) {
          showToast(
            this.toastService, 
            this.multiLanguageService, 
            'success', 
            'common.notify.success-title', 
            'auth.login.re-confirm-success'
          );
        } else {
          showToast(
            this.toastService, 
            this.multiLanguageService, 
            'error', 
            'common.notify.error-title', 
            'common.notify.error-message'
          );
        }
      }, 
      error: () =>
        showToast(
          this.toastService, 
          this.multiLanguageService, 
          'error', 
          'common.notify.error-title', 
          'common.notify.error-message'
        )
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
