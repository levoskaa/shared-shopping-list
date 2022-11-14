import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserGroupViewModelPaginatedListViewModel } from 'src/app/shared/models/generated';
import { AppHttpClient } from '../http-clients/app-http-client';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly usersUrl = 'Users';

  constructor(private readonly http: AppHttpClient) {}

  getUserGroups(
    username: string,
    pageSize?: number,
    offset?: number
  ): Observable<UserGroupViewModelPaginatedListViewModel> {
    return this.http.get(`${this.usersUrl}/${username}/groups`, {
      pageSize,
      offset,
    });
  }
}
