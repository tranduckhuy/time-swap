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
  INVALID_CREDENTIAL_CODE,
  NOT_CONFIRM_CODE, 
  REGISTER_CONFIRM_SUCCESS_CODE, 
  SUCCESS_CODE, 
  TOKEN_EXPIRED_CODE, 
  USER_NOT_EXIST_CODE
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
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

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
    const subscription = this.authService.signin(req).subscribe();
      
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private initialForm() {
    this.form = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  private confirmEmail() {
    const clientUrl = AUTH_CLIENT_URL;
    const decodedToken = decodeURIComponent(this.token());
    const req = {
      token: decodedToken,
      email: this.email()
    }
    const subscription = this.authService.confirmEmail(req, this.email(), clientUrl).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
