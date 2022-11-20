export function decodeJwt(token: string): { [key: string]: string } {
  return JSON.parse(atob(token.split('.')[1]));
}
