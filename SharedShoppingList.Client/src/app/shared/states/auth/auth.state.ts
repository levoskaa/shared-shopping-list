import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { tap } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { TokenViewModel } from '../../models/generated';
import { SignIn, SignUp } from './auth.actions';

interface AuthStateModel {
  accessToken?: string;
  accessTokenExpiryTime?: Date;
  refreshToken?: string;
}

const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

@State({
  name: AUTH_STATE_TOKEN,
  defaults: {
    accessToken: undefined,
    accessTokenExpiryTime: undefined,
    refreshToken: undefined,
  },
})
@Injectable()
export class AuthState {
  @Selector()
  static accessToken(state: AuthStateModel): string | undefined {
    return state.accessToken;
  }

  @Selector()
  static refreshToken(state: AuthStateModel): string | undefined {
    return state.refreshToken;
  }

  @Selector()
  static isAuthenticated(state: AuthStateModel): boolean {
    return (
      state.accessToken !== undefined &&
      state.accessTokenExpiryTime !== undefined &&
      state.accessTokenExpiryTime > new Date()
    );
  }

  constructor(private readonly authService: AuthService) {}

  @Action(SignIn)
  signIn(ctx: StateContext<AuthStateModel>, action: SignIn) {
    return this.authService.signIn(action.payload).pipe(
      tap((result: TokenViewModel) => {
        ctx.patchState({
          accessToken: result.accessToken,
          refreshToken: result.refreshToken,
          accessTokenExpiryTime: result.accessTokenExpiryTime,
        });
      })
    );
  }

  @Action(SignUp)
  signUp(ctx: StateContext<AuthStateModel>, action: SignUp) {
    return this.authService.signUp(action.payload).pipe(
      tap((result: TokenViewModel) => {
        ctx.patchState({
          accessToken: result.accessToken,
          refreshToken: result.refreshToken,
          accessTokenExpiryTime: result.accessTokenExpiryTime,
        });
      })
    );
  }
}
