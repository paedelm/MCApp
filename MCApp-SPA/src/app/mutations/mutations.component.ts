import { Component, OnInit } from '@angular/core';
import { MutationForPage } from '../_models/MutationForPage';
import { Pagination, PaginatedResult } from '../_models/Pagination';
import { UserWithAccounts } from '../_models/UserWithAccounts';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute, Router } from '../../../node_modules/@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Account } from '../_models/Account';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-mutations',
  templateUrl: './mutations.component.html',
  styleUrls: ['./mutations.component.css']
})
export class MutationsComponent implements OnInit {
  mutations: MutationForPage;
  pagination: Pagination;
  user: UserWithAccounts;
  account: Account;
  constructor(private authService: AuthService, private userService: UserService,
     private alertify: AlertifyService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.mutations = data.pgMutations.result;
      this.pagination = data.pgMutations.pagination;
      // if (this.user.accounts.length === 0) {
      //   this.router.navigate(['/home']);
      // }
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMutations();
  }
  loadMutations() {
    this.userService
      .getMutations(this.authService.decodedToken.nameid, this.mutations.account.id,
        this.pagination.currentPage, this.pagination.itemsPerPage)
        .subscribe((res: PaginatedResult<MutationForPage>) => {
          this.mutations = res.result;
          this.pagination = res.pagination;
        }, error => {
          this.alertify.error(error);
        }
      );
  }
/*
        return this.userService.getMutations(this.authService.decodedToken.nameid,
            this.userService.getCurrentAccount(),
            this.pageNumber, this.pageSize)
        .pipe(
            catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return of(null);
            })
        );
*/
}
