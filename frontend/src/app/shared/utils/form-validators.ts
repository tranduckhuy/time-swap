import { AbstractControl, FormGroup } from "@angular/forms";
import { MultiLanguageService } from "../services/multi-language.service";

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