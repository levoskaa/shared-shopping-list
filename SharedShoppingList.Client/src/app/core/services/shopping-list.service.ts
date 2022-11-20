import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreateShoppingListEntryDto,
  ShoppingListEntryViewModel,
  ShoppingListEntryViewModelPaginatedListViewModel,
  UpdateShoppingListEntryDto,
} from 'src/app/shared/models/generated';
import { AppHttpClient } from '../http-clients/app-http-client';

@Injectable({
  providedIn: 'root',
})
export class ShoppingListService {
  private readonly groupsUrl = 'Groups';

  constructor(private readonly http: AppHttpClient) {}

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

  deleteShoppingListEntry(groupId: number, entryId: number): Observable<void> {
    return this.http.delete(
      `${this.groupsUrl}/${groupId}/shopping-list-entries/${entryId}`
    );
  }
}
