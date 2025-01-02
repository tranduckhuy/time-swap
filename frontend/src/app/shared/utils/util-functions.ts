import { ToastService } from "../services/toast.service";
import { MultiLanguageService } from "../services/multi-language.service";

/**
 * Displays a toast notification with a specified type and multi-language message.
 *
 * @param toastService - The service used to display toast notifications.
 * @param multiLanguageService - The service used to get translations for the title and message.
 * @param type - The type of the toast notification. This determines how the toast will be displayed.
 *               It can be one of the following values: 'success', 'error', 'warn', 'info'.
 * @param titleKey - The key for the title to be translated using the MultiLanguageService.
 * @param messageKey - The key for the message to be translated using the MultiLanguageService.
 */
export function showToast(
  toastService: ToastService,
  multiLanguageService: MultiLanguageService,
  type: 'success' | 'error' | 'warn' | 'info',
  titleKey: string,
  messageKey: string
): void {
  const title = multiLanguageService.getTranslatedLang(titleKey);
  const message = multiLanguageService.getTranslatedLang(messageKey);

  if (type === 'success') {
    toastService.success(title, message);
  } else if (type === 'error') {
    toastService.error(title, message);
  } else if (type === 'warn') {
    toastService.warn(title, message);
  } else {
    toastService.info(title, message);
  }
}