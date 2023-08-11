import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
import { Observable, of } from 'rxjs';
import { User } from '../_models/User';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';



@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  providers: [{ provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: true } }]
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public accountService: AccountService,private router: Router, private toastr: ToastrService) { } // Inject Service

  ngOnInit(): void {
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/members'),
      error: err => {
       this.toastr.error(err.error);
      },
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
