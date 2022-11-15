import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreateUserGroupDto,
  UserGroupViewModel,
  UserGroupViewModelPaginatedListViewModel,
} from 'src/app/shared/models/generated';
import { AppHttpClient } from '../http-clients/app-http-client';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly usersUrl = 'Users';

  constructor(private readonly http: AppHttpClient) {}

  createUserGroup(
    username: string,
    dto: CreateUserGroupDto
  ): Observable<UserGroupViewModel> {
    return this.http.post(`${this.usersUrl}/${username}/groups`, dto);
  }

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
