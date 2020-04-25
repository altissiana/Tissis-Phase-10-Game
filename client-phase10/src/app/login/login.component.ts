import { Component, OnInit, Inject } from '@angular/core';
import * as OktaSignIn from '@okta/okta-signin-widget';
import { OKTA_CONFIG, OktaConfig } from '@okta/okta-angular';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  signIn: any;

  constructor() {

    this.signIn = new OktaSignIn({
      baseUrl: environment.oidc.baseUrl,
      clientId: environment.oidc.clientId,
      redirectUri: environment.oidc.redirectUri,
      authParams: {
        pkce: true,
        responseMode: 'query',
        issuer: environment.oidc.issuer,
        display: 'page',
        scopes: ['openid', 'profile', 'email']
      }
    });

    // this.signIn = new OktaSignIn({
    //   baseUrl: 'https://dev-545127.okta.com',
    //   clientId: '0oa57sjliOjRkwaOq4x6',
    //   redirectUri: 'http://localhost:4200/implicit/callback',
    //   authParams: {
    //     pkce: true,
    //     responseMode: 'query',
    //     issuer: 'https://dev-545127.okta.com/oauth2/default',
    //     display: 'page',
    //     scopes: ['openid', 'profile', 'email']
    //   }
    // });
   }

  

   ngOnInit(){
    this.signIn.renderEl(
      {el: '#sign-in-widget'},
      (res) => {
        console.log('okta success response', res);
      },
      (err) => {
        console.error(err);
        throw err;
      },
    );
  }

  ngOnDestroy(){
    this.signIn.remove();
  }

}
