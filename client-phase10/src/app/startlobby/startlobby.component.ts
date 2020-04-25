import { Component, OnInit } from '@angular/core';
import { LobbyService } from '../lobby/lobby.service';
import { Router } from '@angular/router';
import { OktaAuthService } from '@okta/okta-angular';

@Component({
  selector: 'app-startlobby',
  templateUrl: './startlobby.component.html',
  styleUrls: ['./startlobby.component.css']
})
export class StartlobbyComponent implements OnInit {
  lobbyName: string;

  constructor(public oktaAuth: OktaAuthService, private router: Router, private lobbyService: LobbyService) { }

  async onLobbySizeSelect(size: number) {
    const userClaims = await this.oktaAuth.getUser();
    const name = userClaims.given_name;
    const email = userClaims.email;

    this.lobbyService.create(this.lobbyName, size, name, email)
    .subscribe(result => {
      console.log('result', result);

      this.router.navigate(['/lobby', result.id]);
    });
  }

  ngOnInit(): void {
  }

}
