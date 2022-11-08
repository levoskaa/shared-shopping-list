import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { Observable, tap } from 'rxjs';
import { AuthState } from 'src/app/shared/states/auth/auth.state';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private accessToken?: string;

  constructor(private readonly store: Store) {
    // There should only be one instance of this interceptor which gets destroyed
    // when the page is unloaded, so there is no need to unsubscribe.
    this.store
      .select(AuthState.accessToken)
      .pipe(tap((accessToken) => (this.accessToken = accessToken)))
      .subscribe();
  }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    // Clone the request and replace the original headers with
    // cloned headers, updated with the authorization.
    const authReq = request.clone({
      headers: request.headers.set(
        'Authorization',
        `Bearer: ${this.accessToken}`
      ),
    });
    return next.handle(authReq);
  }
}
