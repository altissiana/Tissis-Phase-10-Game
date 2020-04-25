import { Component, OnInit } from '@angular/core';
import { EmailService } from '../email/email.service';
import { OktaAuthService } from '@okta/okta-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tissi',
  templateUrl: './tissi.component.html',
  styleUrls: ['./tissi.component.css']
})
export class TissiComponent implements OnInit {
  message: string;

  constructor(private emailService: EmailService, public oktaAuth: OktaAuthService, private router: Router) { }

  ngOnInit(): void {
  }

  async onSubmit(){
    const userClaims = await this.oktaAuth.getUser();
    const name = userClaims.given_name;
    this.emailService.send(name, this.message)
      .subscribe(result => {
        this.router.navigate(['/dashboard']);
      });
  }

}
