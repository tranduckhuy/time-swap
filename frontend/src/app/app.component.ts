import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TranslateModule } from '@ngx-translate/core';

import { MultiLanguageService } from './shared/services/multi-language.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TranslateModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private readonly multiLanguage = inject(MultiLanguageService);

  ngOnInit(): void {
   this.multiLanguage.updateLanguage(this.multiLanguage.language());
  }
}
