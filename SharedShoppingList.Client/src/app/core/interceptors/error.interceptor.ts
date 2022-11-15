import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { catchError, EMPTY, Observable } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private translate?: TranslateService;

  constructor(
    private readonly injector: Injector,
    private readonly toastService: MessageService
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((response: HttpErrorResponse) => {
        this.displayErrorToast(response);
        return EMPTY;
      })
    );
  }

  private displayErrorToast(response: HttpErrorResponse): void {
    const errors = this.extractErrors(response);
    this.toastService.add({
      severity: 'error',
      summary:
        // TODO: add message to errors array as well (in the backend)
        response.error.message ?? this.translate!.instant('errors.title'),
      detail: errors.join('\n'),
    });
  }

  private extractErrors(response: HttpErrorResponse): string[] {
    this.resolveTranslateService();
    const errors: string[] = [];
    if (response.error.errors) {
      errors.push(...response.error.errors);
    }
    // Push other errors to the array here:
    // ...
    const translatedErrors: string[] = errors.map((error) =>
      this.translate!.instant(`errors.codes.${error}`)
    );
    return translatedErrors;
  }

  private resolveTranslateService(): void {
    // Injecting TranslateService in constructor causes circular dependency error.
    // (Because TranslateModule depends on HttpClientModule.)
    if (!this.translate) {
      this.translate = this.injector.get(TranslateService);
    }
  }
}
