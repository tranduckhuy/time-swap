import { effect, inject, Injectable, signal } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class MultiLanguageService {
  private languageSignal = signal<string>(JSON.parse(window.localStorage.getItem('language') ?? '"vi"'));
  language = this.languageSignal.asReadonly();
  
  private readonly translateService = inject(TranslateService);

  constructor() {
    effect(() => {
      window.localStorage.setItem('language', JSON.stringify(this.languageSignal()));
      this.translateService.use(this.languageSignal());
    });
  }

  get getCurrentLang() {
    return this.languageSignal();
  }

  getTranslatedLang(message: string, params?: any) {
    return this.translateService.instant(message, params);
  }

  updateLanguage(lang: string): void {
    this.languageSignal.update(() => {
      switch (lang) {
        case 'en':
          return 'en';
        default:
          return 'vi';
      }
    });
  }
}
