import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreateShoppingListEntryDto,
  CreateUserGroupDto,
  ShoppingListEntryViewModel,
  ShoppingListEntryViewModelPaginatedListViewModel,
  UpdateShoppingListEntryDto,
  UserGroupViewModel,
  UserGroupViewModelPaginatedListViewModel,
} from 'src/app/shared/models/generated';
import { AppHttpClient } from '../http-clients/app-http-client';

@Injectable({
  providedIn: 'root',
})
export class UserGroupService {
  private readonly usersUrl = 'Users';
  private readonly groupsUrl = 'Groups';

  constructor(private readonly http: AppHttpClient) {}

  createUserGroup(
    username: string,
    dto: CreateUserGroupDto
  ): Observable<UserGroupViewModel> {
    return this.http.post(`${this.usersUrl}/${username}/groups`, dto);
  }

  joinUserGroup(inviteCode: string): Observable<void> {
    return this.http.post(`${this.groupsUrl}/join/${inviteCode}`, {});
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

  createShoppingListEntry(
    groupId: string,
    dto: CreateShoppingListEntryDto
  ): Observable<ShoppingListEntryViewModel> {
    return this.http.post(
      `${this.groupsUrl}/${groupId}/shopping-list-entries`,
      dto
    );
  }

  getShoppingListEntries(
    groupId: number,
    pageSize?: number,
    offset?: number
  ): Observable<ShoppingListEntryViewModelPaginatedListViewModel> {
    return this.http.get(`${this.groupsUrl}/${groupId}/shopping-list-entries`, {
      pageSize,
      offset,
    });
  }

  updateShoppingListEntry(
    groupId: number,
    entryId: number,
    dto: UpdateShoppingListEntryDto
  ): Observable<ShoppingListEntryViewModel> {
    return this.http.put(
      `${this.groupsUrl}/${groupId}/shopping-list-entries/${entryId}`,
      dto
    );
  }
}
