import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { tap } from 'rxjs';
import { SignInDto } from 'src/app/shared/models/generated';
import { SignIn } from 'src/app/shared/states/auth/auth.actions';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['login.component.scss'],
})
export class LoginComponent {
  formControls: Record<keyof SignInDto, FormControl> = {
    username: new FormControl(undefined, Validators.required),
    password: new FormControl(undefined, Validators.required),
  };
  form = new FormGroup(this.formControls);

  constructor(private readonly store: Store, private readonly router: Router) {}

  signIn(): void {
    if (this.form.valid) {
      const formValue = this.form.value;
      this.store
        .dispatch(
          new SignIn({
            username: formValue.username!,
            password: formValue.password!,
          })
        )
        .pipe(tap(() => this.router.navigateByUrl('/')))
        .subscribe();
    }
  }
}
