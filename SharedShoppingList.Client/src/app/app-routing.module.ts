import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignInPageComponent } from './components/sign-in-page/sign-in-page.component';
import { SignUpPageComponent } from './components/sign-up-page/sign-up-page.component';
import { UserGroupDetailsPageComponent } from './components/user-group-details-page/user-group-details-page.component';
import { UserGroupsPageComponent } from './components/user-groups-page/user-groups-page.component';
import { AuthorizedGuard } from './core/guards/authorized.guard';
import { UnauthorizedGuard } from './core/guards/unauthorized.guard';
import { LayoutComponent } from './shared/common-ui/layout/layout.component';

const routes: Routes = [
  {
    path: 'sign-in',
    component: SignInPageComponent,
    canActivate: [UnauthorizedGuard],
  },
  {
    path: 'sign-up',
    component: SignUpPageComponent,
    canActivate: [UnauthorizedGuard],
  },
  {
    path: '',
    component: LayoutComponent,
    canActivateChild: [AuthorizedGuard],
    children: [
      {
        path: '',
        pathMatch: 'full',
        component: UserGroupsPageComponent,
      },
      {
        path: 'group/:id',
        component: UserGroupDetailsPageComponent,
      },
    ],
  },
  {
    path: '**',
    redirectTo: '/',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
