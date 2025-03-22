import { FormGroup } from '@angular/forms';

import { CategoryService } from '../services/category.service';
import { LocationService } from '../services/location.service';
import { FilterService } from '../services/filter.service';
import { MultiLanguageService } from '../services/multi-language.service';

/**
 * Creates default options for the "Date Posted" filter with translated values.
 *
 * This function generates an array of default options for filtering jobs based on the date they
 * were posted. Each option label is translated using the `MultiLanguageService` according to
 * the current application language.
 *
 * @param multiLanguage - An instance of `MultiLanguageService` for translating labels.
 * @returns An array of translated strings representing default "Date Posted" options:
 *  - 'All Date Posted'
 *  - 'Today'
 *  - 'Yesterday'
 *  - 'Last 7 Days'
 *  - 'Last 30 Days'
 */
export function createPostedDateOptions(
  multiLanguageService: MultiLanguageService,
) {
  const keys = [
    'jobs.filter.postedDate.all-posted-date',
    'jobs.filter.postedDate.today',
    'jobs.filter.postedDate.yesterday',
    'jobs.filter.postedDate.last-7-days',
    'jobs.filter.postedDate.last-30-days',
  ];

  return keys.map((key) => multiLanguageService.getTranslatedLang(key));
}

/**
 * Formats a numeric value according to the specified locale.
 * Ideal for converting a number into a localized, human-readable format with thousands separators.
 *
 * @param value - The numeric value to format.
 * @param locale - A string representing the desired locale (e.g., 'en-US' for English, 'vi-VN' for Vietnamese).
 * @returns A string representing the formatted number, including thousands separators and up to two fractional digits, based on the locale.
 */
export function formatNumberValue(value: number, locale: string): string {
  return new Intl.NumberFormat(locale, {
    maximumFractionDigits: 2,
    minimumFractionDigits: 0,
  }).format(value);
}

/**
 * Handles the selection change for dynamic form fields.
 *
 * @param {string} field - The name of the form field.
 * @param {string} value - The selected value from the dropdown.
 * @param {any[]} options - The list of available options for the dropdown.
 * @param {FormGroup} form - The reactive form group to update.
 * @param {FilterService} filterService - Service to handle filtering logic.
 * @param {CategoryService} categoryService - Service to fetch categories.
 * @param {LocationService} [locationService] - Service to fetch locations (optional).
 */
export function handleSelectChange(
  field: string,
  value: string,
  options: any[],
  form: FormGroup,
  filterService: FilterService,
  categoryService: CategoryService,
  locationService?: LocationService,
): void {
  const id = filterService.getOptionId(value, options);
  form.get(field)?.setValue(id);

  if (field === 'industryId') {
    if (id) {
      categoryService.getCategoriesByIndustryId(Number(id)).subscribe(() => {
        form.get('categoryId')?.setValue('');
      });
    } else {
      categoryService.clearCategories();
      form.get('categoryId')?.setValue('');
    }
  }

  if (field === 'cityId' && locationService) {
    if (id) {
      locationService.getWardByCityId(id).subscribe(() => {
        form.get('wardId')?.setValue('');
      });
    } else {
      locationService.clearWards();
      form.get('wardId')?.setValue('');
    }
  }
}
