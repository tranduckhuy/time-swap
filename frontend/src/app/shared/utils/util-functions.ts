import { Subscription } from 'rxjs';

import { MultiLanguageService } from '../services/multi-language.service';
import { LocationService } from '../services/location.service';
import { CategoryService } from '../services/category.service';

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
 * Fetches wards for a specific city by its ID.
 *
 * This function retrieves a list of wards corresponding to a given city ID.
 * The resulting data is typically used to populate a dropdown or a list of options
 * for the user to select a ward within the specified city.
 *
 * @param cityId - A string representing the ID of the city for which wards are fetched.
 * @param locationService - An instance of `LocationService` used to fetch ward data.
 * @returns A `Subscription` object representing the ongoing data-fetching operation.
 */
export function fetchWardsByCityId(
  cityId: string,
  locationService: LocationService,
): Subscription {
  return locationService.getWardByCityId(cityId).subscribe();
}

/**
 * Fetches categories for a specific industry by its ID.
 *
 * This function retrieves a list of categories corresponding to a given industry ID.
 * The resulting data is typically used to populate a dropdown or a list of options
 * for the user to select a category within the specified industry.
 *
 * @param industryId - A number representing the ID of the industry for which categories are fetched.
 * @param categoryService - An instance of `CategoryService` used to fetch category data.
 * @returns A `Subscription` object representing the ongoing data-fetching operation.
 */
export function fetchCategoriesByIndustryId(
  industryId: number,
  categoryService: CategoryService,
): Subscription {
  return categoryService.getCategoriesByIndustryId(industryId).subscribe();
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
