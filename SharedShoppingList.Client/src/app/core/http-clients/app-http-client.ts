import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AppHttpClient {
  constructor(private readonly httpClient: HttpClient) {}

  post<TResponse, TBody>(url: string, body: TBody): Observable<TResponse> {
    return this.httpClient.post<TResponse>(
      `${environment.apiBaseUrl}/${url}`,
      body
    );
  }

  get<TResponse, TParams>(
    url: string,
    params?: TParams
  ): Observable<TResponse> {
    const paramsWithoutNils = params
      ? JSON.parse(JSON.stringify(params))
      : undefined;
    return this.httpClient.get<TResponse>(`${environment.apiBaseUrl}/${url}`, {
      params: paramsWithoutNils,
    });
  }

  put<TResponse, TBody>(url: string, body: TBody): Observable<TResponse> {
    return this.httpClient.put<TResponse>(
      `${environment.apiBaseUrl}/${url}`,
      body
    );
  }

  delete<TResponse>(url: string): Observable<TResponse> {
    return this.httpClient.delete<TResponse>(
      `${environment.apiBaseUrl}/${url}`
    );
  }
}
