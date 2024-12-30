import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastComponent } from "../../../../shared/components/toast/toast.component";
import { PreLoaderComponent } from "../../../../shared/components/pre-loader/pre-loader.component";

import { AuthService } from '../../auth.service';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, ToastComponent, PreLoaderComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  userId = signal<string>('');
  isLoading = signal<boolean>(true);

  private readonly authService = inject(AuthService);
  private readonly toastService = inject(ToastService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  ngOnInit(): void {
    this.initForm();

    setTimeout(() => this.isLoading.set(false), 1000);
  }

  initForm() {
    this.form = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      console.log('Form Invalid!');
      return;
    }
    this.toastService.success('Success', 'Login Successfully!');
    
    const subscription = this.authService.signin(this.form.value).subscribe({
      next: (res) => {
        if (res.data) {
          const { accessToken, refreshToken, expiresIn } = res.data;
          this.authService.saveLocalData(accessToken, refreshToken, expiresIn);
          this.toastService.success('Success', 'Login Successfully!');
          this.router.navigateByUrl('/home');
        } else {
          this.toastService.error('Error', 'Error occurred while executing. Please try again later.');
        }
      },
      error: (error) => {
        console.log(error);
      }
    });

    this.form.reset();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
