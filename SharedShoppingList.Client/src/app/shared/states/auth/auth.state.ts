import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { tap } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { extractUserDataFromToken } from '../../../core/utils/extractUserDataFromToken';
import { getTokenExpiryTime } from '../../../core/utils/getTokenExpiryTime';
import { SignOutDto, TokenViewModel } from '../../models/generated';
import { User } from '../../models/user.models';
import { SignIn, SignOut, SignUp } from './auth.actions';

interface AuthStateModel {
  tokens: {
    accessToken?: string;
    refreshToken?: string;
  };
  user?: User;
}

const defaults: AuthStateModel = {
  tokens: {
    accessToken: undefined,
    refreshToken: undefined,
  },
  user: undefined,
};

const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

@State({
  name: AUTH_STATE_TOKEN,
  defaults,
})
@Injectable()
export class AuthState {
  @Selector()
  static accessToken(state: AuthStateModel): string | undefined {
    return state.tokens.accessToken;
  }

  @Selector()
  static refreshToken(state: AuthStateModel): string | undefined {
    return state.tokens.refreshToken;
  }

  @Selector()
  static isAuthenticated(state: AuthStateModel): boolean {
    return (
      state.tokens.accessToken !== undefined &&
      getTokenExpiryTime(state.tokens.accessToken) > new Date()
    );
  }

  @Selector()
  static user(state: AuthStateModel): User | undefined {
    return state.user;
  }

  constructor(private readonly authService: AuthService) {}

  @Action(SignIn)
  signIn(ctx: StateContext<AuthStateModel>, action: SignIn) {
    return this.authService.signIn(action.payload).pipe(
      tap((result: TokenViewModel) => {
        ctx.patchState({
          tokens: {
            accessToken: result.accessToken,
            refreshToken: result.refreshToken,
          },
          user: extractUserDataFromToken(result.accessToken!),
        });
      })
    );
  }

  @Action(SignUp)
  signUp(ctx: StateContext<AuthStateModel>, action: SignUp) {
    return this.authService.signUp(action.payload).pipe(
      tap((result: TokenViewModel) => {
        ctx.patchState({
          tokens: {
            accessToken: result.accessToken,
            refreshToken: result.refreshToken,
          },
          user: extractUserDataFromToken(result.accessToken!),
        });
      })
    );
  }

  @Action(SignOut)
  signOut(ctx: StateContext<AuthStateModel>) {
    const state = ctx.getState();
    const dto: SignOutDto = {
      refreshToken: state.tokens.refreshToken,
    };
    return this.authService
      .signOut(dto)
      .pipe(tap(() => ctx.setState(defaults)));
  }
}
