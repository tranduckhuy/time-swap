import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { TranslateModule } from '@ngx-translate/core';

import { BannerComponent } from '../../../../shared/components/banner/banner.component';
import { BreadcrumbComponent } from '../../../../shared/components/breadcrumb/breadcrumb.component';
import { PreLoaderComponent } from '../../../../shared/components/pre-loader/pre-loader.component';
import { ToastComponent } from '../../../../shared/components/toast/toast.component';

import { getErrorMessage } from '../../../../shared/utils/form-validators';

import type { PaymentRequestModel } from '../../../../shared/models/api/request/payment-request.model';

import { PaymentService } from './payment.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TranslateModule,
    BannerComponent,
    BreadcrumbComponent,
    PreLoaderComponent,
    ToastComponent,
  ],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css',
})
export class PaymentComponent implements OnInit {
  // ? Form
  form!: FormGroup;

  // ? Loading state
  isLoading = signal<boolean>(true);

  private readonly paymentService = inject(PaymentService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly fb = inject(FormBuilder);
  private readonly destroyRef = inject(DestroyRef);

  ngOnInit(): void {
    this.initialForm();

    const timeOutId = setTimeout(() => this.isLoading.set(false), 800);

    this.destroyRef.onDestroy(() => clearTimeout(timeOutId));
  }

  initialForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [
        '',
        [Validators.required, Validators.pattern(/(84|0[35789])+(\d{8})\b/g)],
      ],
      amount: [
        '',
        [
          Validators.required,
          Validators.min(2000),
          Validators.pattern('^[0-9]*$'),
        ],
      ],
      paymentContent: ['', Validators.required],
      paymentMethodId: ['1', Validators.required],
    });
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

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const req: PaymentRequestModel = {
      amount: this.form.value.amount,
      paymentContent: this.form.value.paymentContent,
      paymentMethodId: this.form.value.paymentMethodId,
    };
    const subscription = this.paymentService.checkoutPayment(req).subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
