import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customCurrency',
  standalone: true
})
export class CustomCurrencyPipe implements PipeTransform {
  private readonly EXCHANGE_RATE = 25383;
  private readonly DEFAULT_LANGUAGE = 'vi';
  private readonly CURRENCIES = {
    vi: '₫',
    en: 'USD'
  } as const;

  transform(
    value: number | string | null | undefined, 
    lang: 'vi' | 'en' = this.DEFAULT_LANGUAGE
  ): string {
    if (!value) {
      return `0 ${this.CURRENCIES[lang]}`;
    }

    const numericValue = this.parseValue(value);
    const { amount, currency } = this.formatCurrency(numericValue, lang);

    return `${this.formatNumber(amount)} ${currency}`;
  }

  private parseValue(value: number | string): number {
    return typeof value === 'string' ? parseFloat(value) : value;
  }

  private formatCurrency(value: number, lang: 'vi' | 'en'): { amount: number; currency: string } {
    if (lang === this.DEFAULT_LANGUAGE) {
      return {
        amount: value,
        currency: this.CURRENCIES.vi
      };
    }
    
    return {
      amount: parseFloat((value / this.EXCHANGE_RATE).toFixed(2)),
      currency: this.CURRENCIES.en
    };
  }

  private formatNumber(value: number): string {
    return new Intl.NumberFormat(undefined, {
      maximumFractionDigits: 2,
      minimumFractionDigits: 0
    }).format(value);
  }
}
