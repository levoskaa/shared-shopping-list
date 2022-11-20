import { Component, Input, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { InviteCodesService } from 'src/app/core/services/invite-codes.service';
import { InviteCodeViewModel } from 'src/app/shared/models/generated';

@Component({
  selector: 'app-invite-codes-list',
  templateUrl: './invite-codes-list.component.html',
  styleUrls: ['./invite-codes-list.component.scss'],
})
export class InviteCodesListComponent implements OnInit {
  @Input() groupId!: number;

  inviteCodes: InviteCodeViewModel[] = [];

  constructor(private readonly inviteCodesService: InviteCodesService) {}

  ngOnInit(): void {
    this.getInviteCode().subscribe();
  }

  regenerateInviteCode(): void {
    this.inviteCodesService
      .regenerate(this.groupId)
      .pipe(tap((inviteCode) => (this.inviteCodes = [inviteCode])))
      .subscribe();
  }

  private getInviteCode(): Observable<InviteCodeViewModel> {
    return this.inviteCodesService
      .getAll(this.groupId)
      .pipe(tap((inviteCode) => (this.inviteCodes = [inviteCode])));
  }
}
