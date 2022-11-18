import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { tap } from 'rxjs';
import { UserGroupService } from 'src/app/core/services/user-group.service';
import {
  DialogClosedEvent,
  DialogCloseType,
} from 'src/app/shared/models/dialog.models';
import {
  CreateShoppingListEntryDto,
  UpdateShoppingListEntryDto,
} from 'src/app/shared/models/generated';

@Component({
  templateUrl: './upsert-shopping-list-entry-dialog.component.html',
})
export class UpsertShoppingListEntryDialogComponent implements OnInit {
  formControls: Record<keyof CreateShoppingListEntryDto, FormControl> = {
    name: new FormControl('', Validators.required),
    quantity: new FormControl('', Validators.required),
  };
  form = new FormGroup(this.formControls);

  constructor(
    private readonly config: DynamicDialogConfig,
    private readonly userGroupService: UserGroupService,
    private readonly dialogRef: DynamicDialogRef
  ) {}

  ngOnInit(): void {
    if (!this.config.data.isNew) {
      this.form.patchValue(this.config.data.shoppingListEntry);
      this.form.markAsPristine();
    }
  }

  submit(): void {
    if (!this.form.valid) {
      return;
    }
    if (this.config.data.isNew) {
      this.insert();
    } else {
      this.update();
    }
  }

  cancel(): void {
    const dialogClosedEvent: DialogClosedEvent = {
      closeType: DialogCloseType.Canceled,
    };
    this.dialogRef.close(dialogClosedEvent);
  }

  private insert(): void {
    const dto: CreateShoppingListEntryDto = {
      name: this.form.value.name,
      quantity: this.form.value.quantity,
    };
    this.userGroupService
      .createShoppingListEntry(this.config.data.groupId, dto)
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

  private update(): void {
    const dto: UpdateShoppingListEntryDto = {
      name: this.form.value.name,
      quantity: this.form.value.quantity,
    };
    this.userGroupService
      .updateShoppingListEntry(
        this.config.data.groupId,
        this.config.data.shoppingListEntry.id,
        dto
      )
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
}
