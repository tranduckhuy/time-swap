import { Injectable, inject, effect, signal } from '@angular/core';

import { TranslateService } from '@ngx-translate/core';

import {
  LANGUAGE,
  ENGLISH,
  VIETNAMESE,
} from '../constants/multi-lang-constants';

@Injectable({
  providedIn: 'root',
})
export class MultiLanguageService {
  private languageSignal = signal<string>(
    JSON.parse(localStorage.getItem(LANGUAGE) ?? `"${VIETNAMESE}"`),
  );
  language = this.languageSignal.asReadonly();

  private readonly translateService = inject(TranslateService);

  constructor() {
    effect(() => {
      if (localStorage.getItem(LANGUAGE)) {
        localStorage.removeItem(LANGUAGE);
      }
      localStorage.setItem(LANGUAGE, JSON.stringify(this.languageSignal()));
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
   * @param key - The translation key to look up.
   * @param params - Optional parameters for interpolation in the translated string.
   * @returns The translated string.
   */
  getTranslatedLang(key: string, params?: any) {
    return this.translateService.instant(key, params);
  }

  /**
   * Updates the application's current language.
   * Supports switching between Vietnamese ('vi') and English ('en').
   *
   * @param lang - The language code to switch to.
   */
  updateLanguage(lang: string): void {
    this.languageSignal.set(lang === VIETNAMESE ? VIETNAMESE : ENGLISH);
  }
}
