import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

import { catchError, Observable, tap, throwError } from 'rxjs';

import { environment } from '../../../../../environments/environment';

import { SUCCESS_CODE } from '../../../../shared/constants/status-code-constants';

import { createHttpParams } from '../../../../shared/utils/request-utils';

import { BaseResponseModel } from '../../../../shared/models/api/base-response.model';
import {
  PaymentRequestModel,
  PaymentReturnRequestModel,
} from '../../../../shared/models/api/request/payment-request.model';

import { ToastHandlingService } from '../../../../shared/services/toast-handling.service';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private httpClient = inject(HttpClient);
  private toastHandlingService = inject(ToastHandlingService);

  private BASE_API_URL = environment.apiBaseUrl;
  private PAYMENT_API_URL = `${this.BASE_API_URL}/payments`;
  private PAYMENT_RETURN_API_URL = `${this.BASE_API_URL}/payments/vnpay-return`;

  checkoutPayment(
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

  processReturn(req: PaymentReturnRequestModel): Observable<BaseResponseModel> {
    const reqParams = createHttpParams(req);
    return this.httpClient
      .get<BaseResponseModel>(this.PAYMENT_RETURN_API_URL, {
        params: reqParams,
      })
      .pipe(
        tap((res) => {
          if (res.statusCode === SUCCESS_CODE && res.data) {
            this.toastHandlingService.handleSuccess('payment.notify.success');
          } else {
            this.toastHandlingService.handleError('payment.notify.failed');
          }
        }),
        catchError((error) => {
          this.toastHandlingService.handleCommonError();
          return throwError(() => error);
        }),
      );
  }
}
