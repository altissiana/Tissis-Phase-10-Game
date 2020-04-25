import { Component, OnInit } from '@angular/core';
import { OktaAuthService } from '@okta/okta-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  name: string;

  constructor(public oktaAuth: OktaAuthService, private router: Router) { }

  async ngOnInit() {
    const accessToken = await this.oktaAuth.getAccessToken();
    console.log('accessToken', accessToken);
    const userClaims = await this.oktaAuth.getUser();
    console.log('user claims', userClaims);
    this.name = userClaims.given_name;
  }

}
