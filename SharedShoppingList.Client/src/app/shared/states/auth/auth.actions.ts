export class SignIn {
  static readonly type = '[Auth] Sign In';
  constructor(public payload: { username: string; password: string }) {}
}
