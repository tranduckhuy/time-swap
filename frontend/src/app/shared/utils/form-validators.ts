import { AbstractControl, FormGroup } from "@angular/forms";
import { MultiLanguageService } from "../services/multi-language.service";

/**
 * Custom validator that checks if the values of two form controls are equal.
 * 
 * @param controlName1 - The name of the first form control to compare.
 * @param controlName2 - The name of the second form control to compare.
 * @returns A validator function that checks if the values are equal.
 */
export function controlValueEqual(controlName1: string, controlName2: string) {
    return (control: AbstractControl) => {
        const val1 = control.get(controlName1)?.value;
        const val2 = control.get(controlName2)?.value;
        if (val1 === val2) {
            return null;
        }
        return {
            controlValueNotEqual: true
        }
    }
}

/**
 * Returns the error message for a specific form control, with translations.
 * 
 * @param controlName - The name of the form control to get the error message for.
 * @param name - The label/name of the field, used in error messages.
 * @param form - The form group that contains the control.
 * @param multiLanguageService - The service used to get translated messages.
 * @returns The translated error message or an empty string if no error.
 */
export function getErrorMessage(controlName: string, name: string, form: FormGroup, multiLanguageService: MultiLanguageService) {
    const control = form.controls[controlName];

    if (control?.hasError('required')) {
        return multiLanguageService.getTranslatedLang('common.form.errors.required', { name });
    }

    if (control?.hasError('email')) {
        return multiLanguageService.getTranslatedLang('common.form.errors.email');
    }

    if (control?.hasError('minLength')) {
        return multiLanguageService.getTranslatedLang('common.form.errors.minLength', {
            name,
            requiredLength: control.errors?.['minlength'].requiredLength
        });
    }

    if (control?.hasError('pattern')) {
        return multiLanguageService.getTranslatedLang('common.form.errors.pattern', { name });
    }

    if (control?.hasError('controlValueNotEqual')) {
        return multiLanguageService.getTranslatedLang('common.form.errors.confirmPassword');
    }

    return '';
}   