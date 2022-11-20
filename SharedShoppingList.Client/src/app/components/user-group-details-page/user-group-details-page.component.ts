import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { tap } from 'rxjs';
import { UserGroupService } from 'src/app/core/services/user-group.service';
import { isInt } from 'src/app/core/utils/isInt';
import { UserGroupDetailsViewModel } from 'src/app/shared/models/generated';
import { AuthState } from 'src/app/shared/states/auth/auth.state';

@Component({
  templateUrl: './user-group-details-page.component.html',
  styleUrls: ['./user-group-details-page.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UserGroupDetailsPageComponent implements OnInit {
  groupId!: number;
  userGroup?: UserGroupDetailsViewModel;

  private username?: string;

  constructor(
    private route: ActivatedRoute,
    private readonly router: Router,
    private readonly userGroupService: UserGroupService,
    private readonly store: Store
  ) {}

  ngOnInit(): void {
    this.initData();
    this.userGroupService
      .getUserGroup(this.username!, this.groupId)
      .pipe(tap((userGroupDetails) => (this.userGroup = userGroupDetails)))
      .subscribe();
  }

  private initData(): void {
    this.username = this.store.selectSnapshot(AuthState.user)?.username;
    const groupId = this.route.snapshot.paramMap.get('id');
    if (groupId !== null && !isInt(groupId)) {
      this.router.navigateByUrl('/');
    }
    this.groupId = parseInt(groupId!);
  }
}
