import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree,
} from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { AuthState } from 'src/app/shared/states/auth/auth.state';

@Injectable({
  providedIn: 'root',
})
export class UnauthorizedGuard
  implements CanActivate, CanActivateChild, CanLoad
{
  private isAuthenticated = false;

  constructor(private readonly store: Store, private readonly router: Router) {
    this.isAuthenticated = this.store.selectSnapshot(AuthState.isAuthenticated);
  }

  canActivate(
    _route: ActivatedRouteSnapshot,
    _state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.performCheck();
  }

  canActivateChild(
    _childRoute: ActivatedRouteSnapshot,
    _state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.performCheck();
  }

  canLoad(
    _route: Route,
    _segments: UrlSegment[]
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.performCheck();
  }

  private performCheck(): boolean {
    if (this.isAuthenticated) {
      this.router.navigateByUrl('/');
    }
    return true;
  }
}
