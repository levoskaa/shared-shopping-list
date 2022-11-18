import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DialogService } from 'primeng/dynamicdialog';
import { filter, map, Observable, switchMap, tap } from 'rxjs';
import { UserGroupService } from 'src/app/core/services/user-group.service';
import { DialogCloseType } from 'src/app/shared/models/dialog.models';
import { ShoppingListEntryViewModel } from 'src/app/shared/models/generated';
import { UpsertShoppingListEntryDialogComponent } from '../upsert-shopping-list-entry-dialog/upsert-shopping-list-entry-dialog.component';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  providers: [DialogService],
})
export class ShoppingListComponent implements OnInit {
  @Input() groupId!: number;

  items: ShoppingListEntryViewModel[] = [];

  constructor(
    private readonly userGroupSerive: UserGroupService,
    private readonly dialogService: DialogService,
    private readonly translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.getShoppingListEntries().subscribe();
  }

  openCreateShoppingListEntryDialog(): void {
    this.openUpsertDialog(true);
  }

  editShoppingListEntry(id: number): void {
    this.openUpsertDialog(false, id);
  }

  deleteShoppingListEntry(id: number): void {}

  private openUpsertDialog(isNew: boolean, shoppingListEntryId?: number): void {
    const dialogRef = this.dialogService.open(
      UpsertShoppingListEntryDialogComponent,
      {
        data: {
          groupId: this.groupId,
          isNew,
          shoppingListEntry: this.items.find(
            (item) => item.id === shoppingListEntryId
          ),
        },
        header: this.translate.instant(
          isNew ? 'shoppingList.createNew' : 'shoppingList.edit'
        ),
        width: '80%',
      }
    );
    dialogRef.onClose
      .pipe(
        filter(
          (dialogClosed) =>
            dialogClosed?.closeType === DialogCloseType.Successful
        ),
        switchMap(() => this.getShoppingListEntries())
      )
      .subscribe();
  }

  private getShoppingListEntries(): Observable<ShoppingListEntryViewModel[]> {
    return this.userGroupSerive.getShoppingListEntries(this.groupId).pipe(
      map((response) => response.items ?? []),
      tap((shoppingListItems) => (this.items = shoppingListItems))
    );
  }
}
