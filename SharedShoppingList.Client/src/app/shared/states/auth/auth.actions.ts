export class SignIn {
  static readonly type = '[Auth] Sign In';
  constructor(public payload: { username: string; password: string }) {}
}

export class SignUp {
  static readonly type = '[Auth] Sign Up';
  constructor(public payload: { username: string; password: string }) {}
}
