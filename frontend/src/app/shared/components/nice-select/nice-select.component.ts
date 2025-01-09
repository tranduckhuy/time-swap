import { Component, computed, inject, input, output, signal } from '@angular/core';
import { NgClass } from '@angular/common';

import { TranslateModule } from '@ngx-translate/core';

import { MultiLanguageService } from '../../services/multi-language.service';

import { StopPropagationDirective } from '../../directives/stop-propagation.directive';

@Component({
  selector: 'app-nice-select',
  standalone: true,
  imports: [TranslateModule, NgClass, StopPropagationDirective],
  templateUrl: './nice-select.component.html',
  styleUrl: './nice-select.component.css'
})
export class NiceSelectComponent {
  // ? Input properties
  listData = input.required<string[]>();
  title = input<string>();
  isFilter = input<boolean>();

  // ? Output for event emitting
  valueChange = output<any>();

  // ? Signal for state management
  searchData = signal<string>('');

  // ? Computed properties
  current = computed(() => {
    if (this.isFilter()) {
      return `${this.multiLanguageService.getTranslatedLang('common.nice-select.filter-by')} ${this.title()}`;
    }
    return this.listData()[0] || '';
  });

  filteredList = computed(() => {
    const normalize = (str: string) => str.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase();

    const data = this.listData || [];
    const searchingData = normalize(this.searchData() || '');
    return data().filter(item => normalize(item).includes(searchingData));
});

  // ? Dependency Injection
  private multiLanguageService = inject(MultiLanguageService);

  onSearch(searchingData: string) {
    this.searchData.set(searchingData);
  }

  onValueChange(value: any) {
    this.valueChange.emit(value);
  }
}
