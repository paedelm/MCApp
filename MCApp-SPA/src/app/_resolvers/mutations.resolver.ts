import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MutationForPage } from '../_models/MutationForPage';
import { AuthService } from '../_services/auth.service';
import { PaginatedResult } from '../_models/Pagination';

@Injectable()
export class MutationsResolver implements Resolve<PaginatedResult<MutationForPage>> {
    pageSize = 9;
    pageNumber = 1;
    constructor(
        private userService: UserService,
        private authService: AuthService,
        private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<PaginatedResult<MutationForPage>> {
        return this.userService.getMutations(this.authService.decodedToken.nameid,
            this.userService.getCurrentAccount().id,
            this.pageNumber, this.pageSize)
        .pipe(
            catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
            })
        );
    }
}
