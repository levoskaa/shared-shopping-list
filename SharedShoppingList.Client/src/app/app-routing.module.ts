import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignInPageComponent } from './components/sign-in-page/sign-in-page.component';
import { SignUpPageComponent } from './components/sign-up-page/sign-up-page.component';
import { UnauthorizedGuard } from './core/guards/unauthorized.guard';

const routes: Routes = [
  {
    path: 'login',
    component: SignInPageComponent,
    canActivate: [UnauthorizedGuard],
  },
  {
    path: 'sign-up',
    component: SignUpPageComponent,
    canActivate: [UnauthorizedGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
