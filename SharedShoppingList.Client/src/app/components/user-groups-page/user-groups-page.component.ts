import { Component, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { map, Observable, tap } from 'rxjs';
import { UsersService } from 'src/app/core/services/users.service';
import { UserGroupViewModel } from 'src/app/shared/models/generated';
import { AuthState } from 'src/app/shared/states/auth/auth.state';

@Component({
  templateUrl: './user-groups-page.component.html',
  styleUrls: ['./user-groups-page.component.scss'],
})
export class UserGroupsPageComponent implements OnInit {
  userGroups: UserGroupViewModel[] = [];

  private username?: string;

  constructor(
    private readonly usersService: UsersService,
    private readonly store: Store
  ) {}

  ngOnInit(): void {
    this.username = this.store.selectSnapshot(AuthState.user)?.username;
    this.getUserGroups().subscribe();
  }

  private getUserGroups(): Observable<UserGroupViewModel[]> {
    return this.usersService.getUserGroups(this.username!).pipe(
      map((response) => response.items ?? []),
      tap((userGroups: UserGroupViewModel[]) => (this.userGroups = userGroups))
    );
  }
}
