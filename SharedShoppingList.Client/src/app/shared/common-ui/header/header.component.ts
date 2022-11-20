import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Select, Store } from '@ngxs/store';
import { MenuItem, PrimeIcons } from 'primeng/api';
import { Observable, tap } from 'rxjs';
import { TextTransformService } from 'src/app/core/services/text-transform.service';
import { User } from '../../models/user.models';
import { SignOut } from '../../states/auth/auth.actions';
import { AuthState } from '../../states/auth/auth.state';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  items!: MenuItem[];
  @Select(AuthState.user) user$!: Observable<User>;

  constructor(
    private readonly translate: TranslateService,
    private readonly store: Store,
    private readonly router: Router,
    private readonly textTransform: TextTransformService
  ) {}

  ngOnInit(): void {
    // The labels are not translated without setTimeout().
    // TODO: try to find a better solution
    setTimeout(() => {
      this.items = [
        {
          label: this.createLabel('menu.signOut'),
          icon: PrimeIcons.SIGN_OUT,
          command: () =>
            this.store
              .dispatch(new SignOut())
              .pipe(tap(() => this.router.navigateByUrl('/sign-in')))
              .subscribe(),
        },
      ];
    });
  }

  private createLabel(rawLabel: string): string {
    const translatedLabel = this.translate.instant(rawLabel);
    return this.textTransform.capitalizeFirst(translatedLabel);
  }
}
