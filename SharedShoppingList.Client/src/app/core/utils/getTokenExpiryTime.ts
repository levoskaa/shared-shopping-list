import { decodeJwt } from './decodeJwt';

export function getTokenExpiryTime(token: string): Date {
  const decodedJwt = decodeJwt(token);
  return new Date(decodedJwt['exp']);
}
