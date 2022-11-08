import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SignInDto, TokenViewModel } from 'src/app/shared/models/generated';
import { AppHttpClient } from '../http-clients/app-http-client';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly authUrl = 'Auth';

  constructor(private readonly httpClient: AppHttpClient) {}

  signIn(dto: SignInDto): Observable<TokenViewModel> {
    return this.httpClient.post(`${this.authUrl}/sign-in`, dto);
  }
}
