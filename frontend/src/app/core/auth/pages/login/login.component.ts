import { Component, DestroyRef, inject, input, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { ToastComponent } from "../../../../shared/components/toast/toast.component";
import { PreLoaderComponent } from "../../../../shared/components/pre-loader/pre-loader.component";

import { getErrorMessage } from '../../../../shared/utils/form-validators';

import { AUTH_CLIENT_URL } from '../../../../shared/constants/auth-constants';
import { 
  NOT_CONFIRM_CODE, 
  REGISTER_CONFIRM_SUCCESS_CODE, 
  SUCCESS_CODE, 
  TOKEN_EXPIRED_CODE 
} from '../../../../shared/constants/status-code-constants';

import { AuthService } from '../../auth.service';
import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

import type { LoginRequestModel } from '../../../../shared/models/api/request/login-request.model';
import type { ReConfirmRequestModel } from '../../../../shared/models/api/request/confirm-request.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, TranslateModule, RouterLink, ToastComponent, PreLoaderComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  // ? Form Properties
  form!: FormGroup;

  // ? State Management
  isLoading = signal<boolean>(true);

  // ? Query Params Properties
  token = input<string>('');
  email = input<string>('');

  // ? Dependency Injection
  private readonly authService = inject(AuthService);
  private readonly toastHandlingService = inject(ToastHandlingService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  ngOnInit(): void {
    if (this.token() && this.email())
      this.confirmEmail();

    this.initialForm();

    const timeOutId = setTimeout(() => this.isLoading.set(false), 1000);
    
    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
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
          } else {
            this.toastHandlingService.handleCommonError();
          }
        },
        error: (error: HttpErrorResponse) => {
          if (error.error.statusCode === NOT_CONFIRM_CODE) {
            this.toastHandlingService.handleWarning('auth.login.not-confirm');
          } else {
            this.toastHandlingService.handleCommonError();
          }
        } 
      });
      
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private initialForm() {
    this.form = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  private confirmEmail() {
    const decodedToken = decodeURIComponent(this.token());
    const req = {
      token: decodedToken,
      email: this.email()
    }
    const subscription = this.authService.confirmEmail(req).subscribe({
      next: (res) => {
        if (res.statusCode === SUCCESS_CODE) {
          this.toastHandlingService.handleSuccess('auth.login.confirm-success');
        } else {
          this.toastHandlingService.handleCommonError();
        }
      },
      error: (error: HttpErrorResponse) => {
        if (error.error.statusCode === TOKEN_EXPIRED_CODE) {
          this.toastHandlingService.handleWarning('auth.login.token-expired');
          this.reConfirmEmail();
        } else {
          this.toastHandlingService.handleCommonError();
        }
      } 
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private reConfirmEmail() {
    const req: ReConfirmRequestModel = {
      email: this.email(),
      clientUrl: AUTH_CLIENT_URL
    }
    const subscription = this.authService.resendConfirmEmail(req).subscribe({
      next: (res) => {
        if (res.statusCode === REGISTER_CONFIRM_SUCCESS_CODE) {
          this.toastHandlingService.handleSuccess('auth.login.re-confirm-success');
        } else {
          this.toastHandlingService.handleCommonError();
        }
      }, 
      error: () => this.toastHandlingService.handleCommonError()
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
