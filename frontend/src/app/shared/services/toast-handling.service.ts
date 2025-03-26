import { Injectable, inject } from '@angular/core';

import { ToastService } from './toast.service';
import { MultiLanguageService } from './multi-language.service';

@Injectable({
  providedIn: 'root',
})
export class ToastHandlingService {
  private toastService = inject(ToastService);
  private multiLanguageService = inject(MultiLanguageService);

  /**
   * Displays a Toast notification with customizable state, title, and content.
   *
   * @param state - The notification state (`success`, `error`, `warn`, `info`).
   * @param titleKey - The title key from the internationalization (i18n) folder.
   * @param messageKey - The message content key from the internationalization (i18n) folder.
   */
  private showToastMessage(
    state: 'success' | 'error' | 'warn' | 'info',
    titleKey: string,
    messageKey: string,
    params?: any,
  ): void {
    const title = this.multiLanguageService.getTranslatedLang(titleKey);
    const message = this.multiLanguageService.getTranslatedLang(
      messageKey,
      params,
    );

    switch (state) {
      case 'success':
        this.toastService.success(title, message);
        break;
      case 'error':
        this.toastService.error(title, message);
        break;
      case 'warn':
        this.toastService.warn(title, message);
        break;
      case 'info':
        this.toastService.info(title, message);
        break;
    }
  }

  /**
   * Displays a success notification with default title.
   *
   * @param messageKey - The notification message key from the internationalization (i18n) folder.
   */
  handleSuccess(messageKey: string, params?: any): void {
    this.showToastMessage(
      'success',
      'common.notify.success-title',
      messageKey,
      params,
    );
  }

  /**
   * Displays an error notification with default title.
   *
   * @param messageKey - The notification message key from the internationalization (i18n) folder.
   */
  handleError(messageKey: string, params?: any): void {
    this.showToastMessage(
      'error',
      'common.notify.error-title',
      messageKey,
      params,
    );
  }

  /**
   * Displays an error notification with default title and message.
   *
   */
  handleCommonError(): void {
    this.showToastMessage(
      'error',
      'common.notify.error-title',
      'common.notify.error-message',
    );
  }

  /**
   * Displays a warning notification with default title.
   *
   * @param messageKey - The notification message key from the internationalization (i18n) folder.
   */
  handleWarning(messageKey: string, params?: any): void {
    this.showToastMessage(
      'warn',
      'common.notify.warning-title',
      messageKey,
      params,
    );
  }

  /**
   * Displays an information notification with default title.
   *
   * @param messageKey - The notification message key from the internationalization (i18n) folder.
   */
  handleInfo(messageKey: string, params?: any): void {
    this.showToastMessage(
      'info',
      'common.notify.info-title',
      messageKey,
      params,
    );
  }

  /**
   * Displays a notification with custom title.
   *
   * @param state - The notification state (`success`, `error`, `warn`, `info`).
   * @param titleKey - The custom title key from the internationalization (i18n) folder.
   * @param messageKey - The notification message key from the internationalization (i18n) folder.
   */
  handleCustomTitle(
    state: 'success' | 'error' | 'warn' | 'info',
    titleKey: string,
    messageKey: string,
    params?: any,
  ): void {
    this.showToastMessage(state, titleKey, messageKey, params);
  }
}
