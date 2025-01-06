import { Component, computed, input, output, signal } from '@angular/core';
import { NgClass } from '@angular/common';

import { StopPropagationDirective } from '../../directives/stop-propagation.directive';

@Component({
  selector: 'app-nice-select',
  standalone: true,
  imports: [StopPropagationDirective, NgClass],
  templateUrl: './nice-select.component.html',
  styleUrl: './nice-select.component.css'
})
export class NiceSelectComponent {
  listData = input.required<string[]>();
  valueChange = output<any>();
  searchData = signal<string>('');

  current = computed(() => this.listData()[0] || '');

  filteredList = computed(() => {
    const data = this.listData() || [];
    const searchingData = this.searchData()?.toLowerCase() || '';
    return data.filter(item => item?.toLowerCase().includes(searchingData));
  });

  onSearch(searchingData: string) {
    this.searchData.set(searchingData);
  }

  onValueChange(value: any) {
    this.valueChange.emit(value);
  }
}
