import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { tap } from 'rxjs';
import { UserGroupService } from 'src/app/core/services/user-group.service';
import {
  DialogClosedEvent,
  DialogCloseType,
} from 'src/app/shared/models/dialog.models';

@Component({
  templateUrl: './join-user-group-dialog.component.html',
  providers: [DialogService],
})
export class JoinUserGroupDialogComponent {
  formControls = {
    inviteCode: new FormControl('', Validators.required),
  };
  form = new FormGroup(this.formControls);

  constructor(
    private readonly dialogRef: DynamicDialogRef,
    private readonly userGroupService: UserGroupService
  ) {}

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const inviteCode: string = this.formControls.inviteCode.value!;
    this.userGroupService
      .joinUserGroup(inviteCode)
      .pipe(
        tap(() => {
          const dialogClosedEvent: DialogClosedEvent = {
            closeType: DialogCloseType.Successful,
          };
          this.dialogRef.close(dialogClosedEvent);
        })
      )
      .subscribe();
  }

  cancel(): void {
    const dialogClosedEvent: DialogClosedEvent = {
      closeType: DialogCloseType.Canceled,
    };
    this.dialogRef.close(dialogClosedEvent);
  }
}
