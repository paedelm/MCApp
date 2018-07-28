import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormBuilder, FormGroup, Validators } from '../../../node_modules/@angular/forms';
import { Router } from '../../../node_modules/@angular/router';
import { BsDatepickerConfig } from '../../../node_modules/ngx-bootstrap';
import { Account } from '../_models/Account';
import { UserService } from '../_services/user.service';
import { UserWithAccounts } from '../_models/UserWithAccounts';
import { AccountForDetailed } from '../_models/AccountForDetailed';

@Component({
  selector: 'app-add-account',
  templateUrl: './add-account.component.html',
  styleUrls: ['./add-account.component.css']
})
export class AddAccountComponent implements OnInit {

  @Output() cancelAddAccount = new EventEmitter();
  addAccountForm: FormGroup;
  account: Account;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router
  ) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createAddAccountForm();
  }

  createAddAccountForm() {
    this.addAccountForm = this.fb.group({
      accountname: ['', Validators.required],
      description: ['', Validators.required],
      percentage: [0.0, Validators.required],
    }, {validator: this.formValidator});
  }

    formValidator(g: FormGroup) {
      const percentage = g.get('percentage').value;
      return null;
      // return  {'mismatch': true};
    }

    addAccount() {
      if (this.addAccountForm.valid) {
        this.account = Object.assign({}, this.addAccountForm.value);
        const userId = this.authService.decodedToken.nameid;
        this.userService.addAccount(userId, this.account).subscribe((acc: AccountForDetailed) => {
          this.authService.clearUserCache();
          this.alertify.success(`${acc.user.knownAs}:Add ${acc.accountname} Successful`);
        }, error => {
          this.alertify.error(error);
        }, () => {
            this.router.navigate(['/accounts']);
        });
      }
    }

    cancel() {
      this.cancelAddAccount.emit(false);
    }
}
