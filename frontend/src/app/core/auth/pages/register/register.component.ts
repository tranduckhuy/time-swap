import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../auth.service';

import { controlValueEqual } from '../../../../shared/utils/form-validators';

import { RegisterRequestModel } from '../../../../shared/models/api/request/register-request.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;

  private authService = inject(AuthService)
  private destroyRef = inject(DestroyRef);
  private fb = inject(FormBuilder);
  private router = inject(Router);

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required], // Nên thêm pattern sđt Việt Nam vào đây -> Làm sau
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, controlValueEqual('password', 'confirmPassword')]] 
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      console.log('Form Invalid!');
      return;
    }

    const req: RegisterRequestModel = {
      ...this.form.value,
      clientUri: 'auth/login'
    };
    const subscription = this.authService.register(req).subscribe({
      next: (res) => {
        console.log(res);
        this.router.navigateByUrl('auth/login');
      },
      error: (error) => console.log(error)
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
