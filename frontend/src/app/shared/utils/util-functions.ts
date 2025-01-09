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
export function createPostedDateOptions(multiLanguageService: MultiLanguageService) {
  return [
    multiLanguageService.getTranslatedLang('jobs.filter.postedDate.all-posted-date'),
    multiLanguageService.getTranslatedLang('jobs.filter.postedDate.today'),
    multiLanguageService.getTranslatedLang('jobs.filter.postedDate.yesterday'),
    multiLanguageService.getTranslatedLang('jobs.filter.postedDate.last-7-days'),
    multiLanguageService.getTranslatedLang('jobs.filter.postedDate.last-30-days'),
  ];
}
