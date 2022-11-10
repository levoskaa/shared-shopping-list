import { Role } from 'src/app/shared/models/role.model';
import { User } from 'src/app/shared/models/user.model';
import { decodeJwt } from './decodeJwt';

export function extractUserDataFromToken(token: string): User {
  const nameClaim: string =
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
  const roleClaim: string =
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const decodedJwt = decodeJwt(token);
  const user: User = {
    id: decodedJwt['sub'],
    username: decodedJwt[nameClaim],
    role: Role[decodedJwt[roleClaim] as keyof typeof Role],
  };
  return user;
}
