import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { tap } from 'rxjs';
import { UsersService } from 'src/app/core/services/users.service';
import {
  DialogClosedEvent,
  DialogCloseType,
} from 'src/app/shared/models/dialog.models';
import { CreateUserGroupDto } from 'src/app/shared/models/generated';
import { AuthState } from 'src/app/shared/states/auth/auth.state';

@Component({
  templateUrl: './new-user-group-dialog.component.html',
})
export class NewUserGroupDialogComponent implements OnInit {
  formControls: Record<keyof CreateUserGroupDto, FormControl> = {
    name: new FormControl(undefined, Validators.required),
  };
  form = new FormGroup(this.formControls);

  private username?: string;

  constructor(
    private readonly usersService: UsersService,
    private readonly store: Store,
    private readonly dialogRef: DynamicDialogRef
  ) {}

  ngOnInit(): void {
    this.username = this.store.selectSnapshot(AuthState.user)?.username;
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    const dto: CreateUserGroupDto = {
      name: this.form.value.name,
    };
    this.usersService
      .createUserGroup(this.username!, dto)
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
