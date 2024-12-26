import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  userId = signal<string>('');

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);
  private router = inject(Router);

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
        this.authService.saveToken(res.accessToken, res.refreshToken);
        this.router.navigateByUrl('/home');
        console.log(res.userId);
      },
      error: (error) => {
        console.log(error);
      }
    });

    this.form.reset();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
