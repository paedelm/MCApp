import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormBuilder, FormGroup, Validators } from '../../../node_modules/@angular/forms';
import { Router } from '../../../node_modules/@angular/router';
import { BsDatepickerConfig } from '../../../node_modules/ngx-bootstrap';
import { Account } from '../_models/Account';
import { UserService } from '../_services/user.service';
import { UserWithAccounts } from '../_models/UserWithAccounts';
import { AccountForDetailed } from '../_models/AccountForDetailed';
import { Mutation } from '../_models/Mutation';
import { MutationForDetailed } from '../_models/MutationForDetailed';

@Component({
  selector: 'app-add-mutation',
  templateUrl: './add-mutation.component.html',
  styleUrls: ['./add-mutation.component.css']
})

export class AddMutationComponent implements OnInit {
  @Input() account: Account;
  @Output() cancelAddMutation = new EventEmitter();
  @Output() exposeBalance = new EventEmitter();
  addMutationForm: FormGroup;
  mutation: Mutation;
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
    this.createAddMutationForm();
  }

  createAddMutationForm() {
    this.addMutationForm = this.fb.group({
      description: ['', Validators.required],
      amount: [0.0, Validators.required],
    }, {validator: this.formValidator});
  }

    formValidator(g: FormGroup) {
      const percentage = g.get('amount').value;
      return null;
      // return  {'mismatch': true};
    }

    addMutation() {
      if (this.addMutationForm.valid) {
        this.mutation = Object.assign({}, this.addMutationForm.value);
        const userId = this.authService.decodedToken.nameid;
        this.userService.addMutation(userId, this.account, this.mutation).subscribe((mut: MutationForDetailed) => {
          this.authService.clearUserCache();
          this.exposeBalance.emit(mut.balance);
          this.alertify.success(`${mut.account.user.knownAs} new balance: ${mut.balance}:Add ${mut.amount} Successful`);
          this.cancel();
        }, error => {
          this.alertify.error(error);
        });
      }
    }

    cancel() {
      this.cancelAddMutation.emit(false);
    }
}

