import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { tap } from 'rxjs';
import { SignUp } from 'src/app/shared/states/auth/auth.actions';
import { matchingValuesValidator } from '../../core/validators/matchingValuesValidator';

@Component({
  templateUrl: './sign-up-page.component.html',
})
export class SignUpPageComponent {
  formControls = {
    username: new FormControl(undefined, Validators.required),
    password: new FormGroup(
      {
        password1: new FormControl(undefined, Validators.required),
        password2: new FormControl(undefined, Validators.required),
      },
      matchingValuesValidator('password1', 'password2')
    ),
  };
  form = new FormGroup(this.formControls);

  constructor(private readonly store: Store, private readonly router: Router) {}

  signUp(): void {
    if (this.form.valid) {
      const formValue = this.form.value;
      this.store
        .dispatch(
          new SignUp({
            username: formValue.username!,
            password: formValue.password?.password1!,
          })
        )
        .pipe(tap(() => this.router.navigateByUrl('/')))
        .subscribe();
    }
  }
}
