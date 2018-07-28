import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute, Router } from '../../../node_modules/@angular/router';
import { UserWithAccounts } from '../_models/UserWithAccounts';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent implements OnInit {
  user: UserWithAccounts;
  constructor(private authService: AuthService, private userService: UserService,
     private alertify: AlertifyService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data.user;
      if (!this.user.fromCache) {
        this.authService.updateUserCache(this.user);
      }
      if (this.user.accounts.length === 0) {
        this.router.navigate(['/home']);
      }
    });
  }

  newAccount(userId: number) {
    console.log(`new account for user: ${userId}`);
  }

}
