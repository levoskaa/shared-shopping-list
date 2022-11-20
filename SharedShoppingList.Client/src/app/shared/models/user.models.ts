export enum Role {
  user = 'user',
  admin = 'admin',
}

export interface User {
  id: string;
  username: string;
  role: Role;
}
