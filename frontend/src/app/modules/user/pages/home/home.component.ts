import {
  Component,
  computed,
  DestroyRef,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';
import Swal from 'sweetalert2';

import { CustomCurrencyPipe } from '../../../../shared/pipes/custom-currency.pipe';

import { ToastComponent } from '../../../../shared/components/toast/toast.component';
import { NiceSelectComponent } from '../../../../shared/components/nice-select/nice-select.component';

import {
  PaymentRequestModel,
  PaymentReturnRequestModel,
} from '../../../../shared/models/api/request/payment-request.model';

import { ZERO } from '../../../../shared/constants/common-constants';
import {
  ENGLISH,
  VIETNAMESE,
} from '../../../../shared/constants/multi-lang-constants';
import {
  STANDARD_PRICE,
  BASIC_PRICE,
  PREMIUM_PRICE,
  REQUIRED_PARAMS,
} from '../../../../shared/constants/subscription-constant';

import { PaymentService } from '../payment/payment.service';
import { MultiLanguageService } from '../../../../shared/services/multi-language.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    RouterLink,
    TranslateModule,
    CustomCurrencyPipe,
    ToastComponent,
    NiceSelectComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  private readonly paymentService = inject(PaymentService);
  private readonly multiLanguageService = inject(MultiLanguageService);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  industries: string[] = [
    this.multiLanguageService.getTranslatedLang('home.banner.categories.all'),
    this.multiLanguageService.getTranslatedLang(
      'home.banner.categories.teaching',
    ),
    this.multiLanguageService.getTranslatedLang('home.banner.categories.it'),
    this.multiLanguageService.getTranslatedLang('home.banner.categories.other'),
  ];
  industriesSignal = signal<string[]>(this.industries);

  lang = computed(() =>
    this.multiLanguageService.language() === VIETNAMESE ? VIETNAMESE : ENGLISH,
  );

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      if (this.hasRequiredParams(params)) {
        this.processPaymentFromUrl(params);
      }
    });
  }

  packageSelect(packagePurchase: number) {
    Swal.fire({
      title: this.multiLanguageService.getTranslatedLang(
        'home.pricing-plans.confirmation.heading',
      ),
      text: this.multiLanguageService.getTranslatedLang(
        'home.pricing-plans.confirmation.sub-heading',
      ),
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: this.multiLanguageService.getTranslatedLang(
        'home.pricing-plans.confirmation.confirm',
      ),
      cancelButtonText: this.multiLanguageService.getTranslatedLang(
        'home.pricing-plans.confirmation.cancel',
      ),
    }).then((result) => {
      if (result.isConfirmed) {
        this.processPayment(packagePurchase);
      } else {
        Swal.fire({
          title: 'Cancelled',
          text: this.multiLanguageService.getTranslatedLang(
            'home.pricing-plans.cancel-message',
          ),
          icon: 'error',
        });
      }
    });
  }

  private processPayment(packagePurchase: number) {
    let amount: number;
    let packageName: string;

    switch (packagePurchase) {
      case 1:
        amount = PREMIUM_PRICE;
        packageName = this.multiLanguageService.getTranslatedLang(
          'home.pricing-plans.premium',
        );
        break;
      case 2:
        amount = STANDARD_PRICE;
        packageName = this.multiLanguageService.getTranslatedLang(
          'home.pricing-plans.advanced',
        );
        break;
      case 3:
        amount = BASIC_PRICE;
        packageName = this.multiLanguageService.getTranslatedLang(
          'home.pricing-plans.basic',
        );
        break;
      default:
        amount = ZERO;
        packageName = this.multiLanguageService.getTranslatedLang(
          'home.pricing-plans.free',
        );
        break;
    }

    const paymentContent: string = this.multiLanguageService.getTranslatedLang(
      'home.pricing-plans.payment-content',
      {
        packageName,
        amount,
      },
    );
    const req: PaymentRequestModel = {
      amount: amount,
      paymentContent: paymentContent,
      paymentMethodId: 1,
    };

    const subscription = this.paymentService.checkoutPayment(req).subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  private hasRequiredParams(params: any): boolean {
    return REQUIRED_PARAMS.some((param) => params[param] !== undefined);
  }

  private processPaymentFromUrl(params: any) {
    const req: Record<string, any> = {};
    REQUIRED_PARAMS.forEach((param) => {
      if (params[param] !== undefined) {
        req[param] = params[param];
      }
    });

    const subscription = this.paymentService
      .processReturn(req as PaymentReturnRequestModel)
      .subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
