import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { LANGUAGE, VIETNAMESE } from './shared/constants/multi-lang-constants';

import { MultiLanguageService } from './shared/services/multi-language.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TranslateModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private readonly multiLanguageService = inject(MultiLanguageService);

  ngOnInit(): void {
   this.multiLanguageService.updateLanguage(localStorage.getItem(LANGUAGE) ?? VIETNAMESE);
  }
}
