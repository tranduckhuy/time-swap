import { inject, Injectable } from '@angular/core';

import { forkJoin } from 'rxjs';

import { IndustryService } from './industry.service';
import { CategoryService } from './category.service';
import { LocationService } from './location.service';

@Injectable({
  providedIn: 'root',
})
export class FilterService {
  private readonly industryService = inject(IndustryService);
  private readonly categoryService = inject(CategoryService);
  private readonly locationService = inject(LocationService);

  loadSelectOptions() {
    return forkJoin([
      this.industryService.getAllIndustries(),
      this.categoryService.getAllCategories(),
      this.locationService.getAllCities(),
      this.locationService.getWardByCityId('0'),
    ]);
  }

  getOptionId(value: string, options: any[]): string {
    return (
      options.find(
        (option) =>
          option.name === value ||
          option.fullLocation === value ||
          option.industryName === value ||
          option.categoryName === value,
      )?.id || ''
    );
  }
}
