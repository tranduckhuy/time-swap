import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { catchError, map, Observable, tap, throwError } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import {
  CANCEL_TRANSACTION,
  NOT_ENOUGH_BALANCE,
  PAYMENT_TIMEOUT,
  SUCCESS_CODE,
  USER_NOT_ENOUGH_BALANCE,
} from '../../../../shared/constants/status-code-constants';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import type { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import type {
  PaymentRequestModel,
  PayOsReturnRequestModel,
  VnPayReturnRequestModel,
  SubscriptionPlanRequestModel,
} from '../../../../shared/models/api/request/payment-request.model';

import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);

  private BASE_API_URL = environment.apiBaseUrl;
  private BASE_AUTH_API_URL = environment.apiAuthBaseUrl;
  private PAYMENT_API_URL = `${this.BASE_API_URL}/payments`;
  private VN_PAY_RETURN_API_URL = `${this.BASE_API_URL}/payments/vnpay-return`;
  private PAY_OS_RETURN_API_URL = `${this.BASE_API_URL}/payments/payos-return`;
  private CHANGE_SUBSCRIPTION_PLAN_API_URL = `${this.BASE_AUTH_API_URL}/users/subscription`;

  handlingTransaction(
    req: PaymentRequestModel,
  ): Observable<BaseResponseModel<string>> {
    return this.httpClient
      .post<BaseResponseModel<string>>(
        this.PAYMENT_API_URL,
        JSON.stringify(req),
        {
          headers: {
            'Content-Type': 'application/json',
          },
        },
      )
      .pipe(
        tap((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            window.location.href = res.data;
          } else {
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError((error) => {
          if (error.status === 401) {
            this.toastHandlingService.handleWarning('payment.notify.not-login');
            return throwError(() => error);
          }
          this.toastHandlingService.handleCommonError();
          return throwError(() => error);
        }),
      );
  }

  changeSubscriptionPlan(
    req: SubscriptionPlanRequestModel,
  ): Observable<BaseResponseModel<void>> {
    return this.httpClient
      .put<BaseResponseModel<void>>(
        this.CHANGE_SUBSCRIPTION_PLAN_API_URL,
        JSON.stringify(req),
        {
          headers: {
            'Content-Type': 'application/json',
          },
        },
      )
      .pipe(
        tap((res) => {
          if (res.statusCode === SUCCESS_CODE) {
            this.toastHandlingService.handleSuccess(
              'home.pricing-plans.notify.success',
            );
          } else if (res.statusCode === USER_NOT_ENOUGH_BALANCE) {
            this.toastHandlingService.handleWarning(
              'home.pricing-plans.notify.not-enough-balance',
            );
          } else {
            this.toastHandlingService.handleCommonError();
          }
        }),
        catchError((error) => {
          if (error.status === 401) {
            this.toastHandlingService.handleWarning('payment.notify.not-login');
            return throwError(() => error);
          }
          this.toastHandlingService.handleCommonError();
          return throwError(() => error);
        }),
      );
  }

  processVnPayReturn(
    req: VnPayReturnRequestModel,
  ): Observable<BaseResponseModel> {
    return this.handlePaymentReturn(this.VN_PAY_RETURN_API_URL, req);
  }

  processPayOsReturn(
    req: PayOsReturnRequestModel,
  ): Observable<BaseResponseModel> {
    return this.handlePaymentReturn(this.PAY_OS_RETURN_API_URL, req);
  }

  private handlePaymentReturn(
    apiUrl: string,
    req: any,
  ): Observable<BaseResponseModel> {
    const reqParams = createHttpParams(req);
    return this.httpClient
      .get<BaseResponseModel>(apiUrl, { params: reqParams })
      .pipe(
        tap((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            this.toastHandlingService.handleSuccess('payment.notify.success');
          } else {
            switch (res.statusCode) {
              case PAYMENT_TIMEOUT:
                this.toastHandlingService.handleWarning(
                  'payment.notify.time-out',
                );
                break;
              case CANCEL_TRANSACTION:
                this.toastHandlingService.handleInfo(
                  'payment.notify.cancel-transaction',
                );
                break;
              case NOT_ENOUGH_BALANCE:
                this.toastHandlingService.handleWarning(
                  'payment.notify.not-enough-balance',
                );
                break;
              default:
                this.toastHandlingService.handleError('payment.notify.failed');
            }
          }
        }),
        catchError((error) => {
          this.toastHandlingService.handleCommonError();
          return throwError(() => error);
        }),
      );
  }
}
