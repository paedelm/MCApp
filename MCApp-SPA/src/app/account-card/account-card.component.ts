import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Account } from '../_models/Account';

@Component({
  selector: 'app-account-card',
  templateUrl: './account-card.component.html',
  styleUrls: ['./account-card.component.css']
})
export class AccountCardComponent implements OnInit {
  @Input() account: Account;
  mutationMode = false;
  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  sendLike(id: number) {
    this.userService.sendLike(this.authService.decodedToken.nameid, id).subscribe(data => {
      this.alertify.success('You have liked: ' + this.account.accountname);
    }, error => {
      this.alertify.error(error);
    });
  }
  mutationToggle() {
    this.mutationMode = true;
    this.setCurrentAccount();
  }

  cancelMutationMode(mutationMode: boolean) {
    this.mutationMode = mutationMode;
  }
  updateBalance(balance: number) {
    this.account.balance = balance;
  }
  setCurrentAccount() {
    this.userService.setCurrentAccount(this.account);
  }
}
