import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Store } from '@ngxs/store';
import { MenuItem, PrimeIcons } from 'primeng/api';
import { tap } from 'rxjs';
import { SignOut } from '../../states/auth/auth.actions';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  items!: MenuItem[];

  constructor(
    private readonly translate: TranslateService,
    private readonly store: Store,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.items = [
      {
        label: this.translate.instant('menu.signOut'),
        icon: PrimeIcons.SIGN_OUT,
        command: () =>
          this.store
            .dispatch(new SignOut())
            .pipe(tap(() => this.router.navigateByUrl('/sign-in')))
            .subscribe(),
      },
    ];
  }
}
