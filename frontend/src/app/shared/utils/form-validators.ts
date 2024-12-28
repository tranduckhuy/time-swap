import { AbstractControl } from "@angular/forms";

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