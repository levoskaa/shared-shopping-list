import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';

// The FormGroup's listed controls must have equal values
export function matchingValuesValidator(
  ...controlNames: string[]
): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!(control instanceof FormGroup) || controlNames.length < 2) {
      return null;
    }

    const form = control as FormGroup;
    let prevValue = form.controls[controlNames[0]].value;
    for (let i = 1; i < controlNames.length; i++) {
      const currentValue = form.controls[controlNames[i]].value;
      if (prevValue !== currentValue) {
        return { matchingValues: false };
      }
      prevValue = currentValue;
    }

    return null;
  };
}
