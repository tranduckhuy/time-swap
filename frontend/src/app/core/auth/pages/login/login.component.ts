import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastComponent } from "../../../../shared/components/toast/toast.component";

import { AuthService } from '../../auth.service';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, ToastComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  userId = signal<string>('');

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);
  private toastService = inject(ToastService);

  ngOnInit(): void {
    this.initForm();
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
