import { Injectable } from '@angular/core';
import { Action, State, StateContext, StateToken } from '@ngxs/store';
import { tap } from 'rxjs';
import { AuthService } from 'src/app/core/auth/auth.service';
import { TokenViewModel } from '../../models/generated';
import { SignIn } from './auth.actions';

interface AuthStateModel {
  accessToken: string | undefined;
  refreshToken: string | undefined;
}

const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

@State({
  name: AUTH_STATE_TOKEN,
  defaults: {
    accessToken: undefined,
    refreshToken: undefined,
  },
})
@Injectable()
export class AuthState {
  constructor(private readonly authService: AuthService) {}

  @Action(SignIn)
  login(ctx: StateContext<AuthStateModel>, action: SignIn) {
    return this.authService.signIn(action.payload).pipe(
      tap((result: TokenViewModel) => {
        ctx.patchState({
          accessToken: result.accessToken,
          refreshToken: result.refreshToken,
        });
      })
    );
  }
}
