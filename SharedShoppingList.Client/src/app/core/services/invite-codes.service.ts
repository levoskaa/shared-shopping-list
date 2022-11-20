import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InviteCodeViewModel } from 'src/app/shared/models/generated';
import { AppHttpClient } from '../http-clients/app-http-client';

@Injectable({
  providedIn: 'root',
})
export class InviteCodesService {
  private readonly groupsUrl = 'Groups';

  constructor(private readonly http: AppHttpClient) {}

  regenerate(groupId: number): Observable<InviteCodeViewModel> {
    return this.http.post(`${this.groupsUrl}/${groupId}/invite-codes`, {});
  }

  getAll(groupId: number): Observable<InviteCodeViewModel> {
    return this.http.get(`${this.groupsUrl}/${groupId}/invite-codes`);
  }
}
