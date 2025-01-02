import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { ToastComponent } from "../../../../shared/components/toast/toast.component";
import { PreLoaderComponent } from "../../../../shared/components/pre-loader/pre-loader.component";

import { getErrorMessage } from '../../../../shared/utils/form-validators';

import type { LoginRequestModel } from '../../../../shared/models/api/request/login-request.model';

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

  private readonly authService = inject(AuthService);
  private readonly toastService = inject(ToastService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  ngOnInit(): void {
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
    const subscription = this.authService.signin(req).subscribe({
      next: (res) => {
        if (res.data) {
          const { accessToken, refreshToken, expiresIn } = res.data;
          this.authService.saveLocalData(accessToken, refreshToken, expiresIn);
          this.router.navigateByUrl('/home');
        } else {
          this.toastService.error('Error', this.multiLanguageService.getTranslatedLang('common.messages.error'));
        }
      },
      error: () => this.toastService.error('Error', this.multiLanguageService.getTranslatedLang('common.messages.error'))
    });

    this.form.reset();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
