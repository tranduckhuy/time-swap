import { MultiLanguageService } from '../services/multi-language.service';

/**
 * Creates default filter options for industries, categories, cities, and wards with translated values.
 * 
 * This function uses the `MultiLanguageService` to dynamically translate filter option labels 
 * based on the current language. It returns an object containing default filter values with 
 * translated names.
 * 
 * @param multiLanguage - An instance of `MultiLanguageService` for translating labels.
 * @returns An object with default filter options:
 *  - `industries`: Default option for industries with a translated label.
 *  - `categories`: Default option for categories with a translated label.
 *  - `cities`: Default option for cities with a translated label.
 *  - `wards`: Default option for wards with a translated label and full location name.
 */
export function createFilterOptions(multiLanguage: MultiLanguageService) {
  return {
    industries: { id: 0, industryName: multiLanguage.getTranslatedLang('jobs.filter.industry') },
    categories: { id: 0, categoryName: multiLanguage.getTranslatedLang('jobs.filter.category') },
    cities: { id: '', name: multiLanguage.getTranslatedLang('jobs.filter.city') },
    wards: { id: '', name: multiLanguage.getTranslatedLang('jobs.filter.ward'), fullLocation: multiLanguage.getTranslatedLang('jobs.filter.ward') },
  };
}

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
export function createPostedDateOptions(multiLanguage: MultiLanguageService) {
  return [
    multiLanguage.getTranslatedLang('jobs.filter.postedDate.all-posted-date'),
    multiLanguage.getTranslatedLang('jobs.filter.postedDate.today'),
    multiLanguage.getTranslatedLang('jobs.filter.postedDate.yesterday'),
    multiLanguage.getTranslatedLang('jobs.filter.postedDate.last-7-days'),
    multiLanguage.getTranslatedLang('jobs.filter.postedDate.last-30-days'),
  ];
}
