import { Pipe, PipeTransform } from '@angular/core';
import { formatNumberValue } from '../utils/util-functions';

@Pipe({
  name: 'thousand',
  standalone: true
})
export class ThousandPipe implements PipeTransform {
  private readonly DEFAULT_LANGUAGE = 'vi' as const;
  private readonly LOCALES = {
    vi: 'vi-VN',
    en: 'en-US'
  } as const

  transform(
    value: number | string | null | undefined, 
    lang: 'vi' | 'en' = this.DEFAULT_LANGUAGE
  ): string {
    if (!value) 
      return '';
    
    const numericValue = this.parseValue(value);
    if (isNaN(numericValue))
      return '';

    const locale = this.LOCALES[lang];

    return formatNumberValue(numericValue, locale);
  }

  private parseValue(value: number | string): number {
    const parsedValue = typeof value === 'string' ? parseFloat(value) : value;
    return isNaN(parsedValue) ? NaN : parsedValue; 
  }
}
