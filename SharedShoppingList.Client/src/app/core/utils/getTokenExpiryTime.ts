import { decodeJwt } from './decodeJwt';

export function getTokenExpiryTime(token: string): Date {
  const decodedJwt = decodeJwt(token);
  const secondsSinceEpoch = parseInt(decodedJwt['exp']);
  // JavaScript Date needs milliseconds, JWT expiry is defined in seconds.
  return new Date(secondsSinceEpoch * 1000);
}
