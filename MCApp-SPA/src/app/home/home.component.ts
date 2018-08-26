import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  values: any;
  constructor(public authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const newAccount = params['newaccount'];
      if (newAccount) {
        this.registerToggle();
      }
    });
  }


  registerToggle() {
    this.registerMode = true;
  }
  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
  cancelAddAccountMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
}
