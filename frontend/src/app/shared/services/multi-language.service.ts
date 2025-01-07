import { effect, inject, Injectable, signal } from '@angular/core';

import { TranslateService } from '@ngx-translate/core';

import { ENGLISH, LANGUAGE, VIETNAMESE } from '../constants/multi-lang-constants';

@Injectable({
  providedIn: 'root'
})
export class MultiLanguageService {
  private languageSignal = signal<string>(JSON.parse(window.localStorage.getItem(LANGUAGE) ?? '"vi"'));
  language = this.languageSignal.asReadonly();
  
  private readonly translateService = inject(TranslateService);

  constructor() {
    effect(() => {
      window.localStorage.setItem(LANGUAGE, JSON.stringify(this.languageSignal()));
      this.translateService.use(this.languageSignal());
    });
  }

  /**
   * Retrieves the current active language.
   * 
   * @returns The current language code ('en' or 'vi').
   */
  get currentLang() {
    return this.languageSignal();
  }

  /**
   * Translates a message key into the current language.
   * 
   * @param value - The translation key to look up.
   * @param params - Optional parameters for interpolation in the translated string.
   * @returns The translated string.
   */
  getTranslatedLang(value: string, params?: any) {
    return this.translateService.instant(value, params);
  }

  /**
   * Updates the application's current language.
   * Supports switching between Vietnamese ('vi') and English ('en').
   * 
   * @param lang - The language code to switch to.
   */
  updateLanguage(lang: string): void {
    this.languageSignal.update(() => {
      if (lang === VIETNAMESE) {
        return VIETNAMESE;
      } else {
        return ENGLISH;
      }
    });
  }
}
