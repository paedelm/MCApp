import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Account } from '../_models/Account';
import { Injectable } from '@angular/core';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';
import { UserWithAccounts } from '../_models/UserWithAccounts';

@Injectable()
export class AccountListResolver implements Resolve<UserWithAccounts> {
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<UserWithAccounts> {
    const id =
      this.authService.decodedToken == null
        ? 0
        : this.authService.decodedToken.nameid;
    if (!this.authService.currentUser) {
      return this.userService.getAccounts(id).pipe(
        catchError(error => {
          this.alertify.error('Problem retrieving data');
          this.router.navigate(['/home']);
          return of(null);
        })
      );
    } else {
        return of(this.authService.currentUser);
    }
  }
}
