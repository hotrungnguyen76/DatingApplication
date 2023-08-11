import { AccountService } from './_services/account.service';
import { User } from './_models/User';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';

  constructor(public accountService: AccountService){

  }
  ngOnInit(): void {
    this.checkLogin();
  }

  checkLogin(): void {
    var userString = localStorage.getItem('user');
    if(userString == null) {
      return;
    }
    var user:User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
