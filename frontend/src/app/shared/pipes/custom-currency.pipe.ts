import { Pipe, PipeTransform } from '@angular/core';

import { formatNumberValue } from '../utils/util-functions';

@Pipe({
  name: 'customCurrency',
  standalone: true,
})
export class CustomCurrencyPipe implements PipeTransform {
  private readonly EXCHANGE_RATE = 25383 as const;
  private readonly DEFAULT_LANGUAGE = 'vi' as const;
  private readonly CURRENCIES = {
    vi: '₫',
    en: 'USD',
  } as const;

  transform(
    value: number | string | null | undefined,
    lang: 'vi' | 'en' = this.DEFAULT_LANGUAGE,
  ): string {
    if (!value) return `0 ${this.CURRENCIES[lang]}`;

    const numericValue = this.parseValue(value);
    if (isNaN(numericValue)) return `0 ${this.CURRENCIES[lang]}`;

    const { amount, currency, locale } = this.formatCurrency(
      numericValue,
      lang,
    );

    return `${formatNumberValue(amount, locale)} ${currency}`;
  }

  private parseValue(value: number | string): number {
    const parsedValue = typeof value === 'string' ? parseFloat(value) : value;
    return isNaN(parsedValue) ? NaN : parsedValue;
  }

  private formatCurrency(
    value: number,
    lang: 'vi' | 'en',
  ): { amount: number; currency: string; locale: string } {
    if (lang === this.DEFAULT_LANGUAGE) {
      return {
        amount: value,
        currency: this.CURRENCIES.vi,
        locale: 'vi-VN',
      };
    }

    return {
      amount: parseFloat((value / this.EXCHANGE_RATE).toFixed(4)),
      currency: this.CURRENCIES.en,
      locale: 'en-US',
    };
  }
}
