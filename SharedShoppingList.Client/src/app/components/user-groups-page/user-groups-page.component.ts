import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Store } from '@ngxs/store';
import { DialogService } from 'primeng/dynamicdialog';
import { filter, map, Observable, switchMap, tap } from 'rxjs';
import { UsersService } from 'src/app/core/services/users.service';
import { DialogCloseType } from 'src/app/shared/models/dialog.models';
import { UserGroupViewModel } from 'src/app/shared/models/generated';
import { AuthState } from 'src/app/shared/states/auth/auth.state';
import { NewUserGroupDialogComponent } from '../new-user-group-dialog/new-user-group-dialog.component';

@Component({
  templateUrl: './user-groups-page.component.html',
  styleUrls: ['./user-groups-page.component.scss'],
  providers: [DialogService],
})
export class UserGroupsPageComponent implements OnInit {
  userGroups: UserGroupViewModel[] = [];

  private username?: string;

  constructor(
    private readonly usersService: UsersService,
    private readonly store: Store,
    public readonly dialogService: DialogService,
    private readonly translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.username = this.store.selectSnapshot(AuthState.user)?.username;
    this.getUserGroups().subscribe();
  }

  openCreateUserGroupDialog(): void {
    const dialogRef = this.dialogService.open(NewUserGroupDialogComponent, {
      header: this.translate.instant('userGroups.createNew'),
      width: '80%',
    });
    dialogRef.onClose
      .pipe(
        filter(
          (dialogClosed) =>
            dialogClosed?.closeType === DialogCloseType.Successful
        ),
        switchMap(() => this.getUserGroups())
      )
      .subscribe();
  }

  private getUserGroups(): Observable<UserGroupViewModel[]> {
    return this.usersService.getUserGroups(this.username!).pipe(
      map((response) => response.items ?? []),
      tap((userGroups: UserGroupViewModel[]) => (this.userGroups = userGroups))
    );
  }
}
