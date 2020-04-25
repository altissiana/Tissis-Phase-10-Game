import { NgModule } from '@angular/core';
import { Routes, RouterModule, Router } from '@angular/router';
import { SignupComponent } from './signup/signup.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component'
import { FindlobbyComponent } from './findlobby/findlobby.component';
import { StartlobbyComponent } from './startlobby/startlobby.component';
import { TissiComponent } from './tissi/tissi.component';
import { OktaAuthGuard, OKTA_CONFIG, OktaCallbackComponent } from '@okta/okta-angular';
import { LobbyComponent } from './lobby/lobby.component';
import { GameComponent } from './game/game.component';
import { environment } from 'src/environments/environment';

export function onAuthRequired(oktaAuth, injector) {
    console.log('injector', injector);
    const router = injector.get(Router);
    router.navigate(['/login']);
}

const routes: Routes = [
  { path: 'signup', component: SignupComponent},
  { path: 'home', component: HomeComponent},
  { path: 'login', component: LoginComponent},
  { path: 'dashboard', component: DashboardComponent, canActivate: [OktaAuthGuard], data: {onAuthRequired: onAuthRequired}},
  { path: 'startlobby', component: StartlobbyComponent, canActivate: [OktaAuthGuard], data: {onAuthRequired: onAuthRequired}},
  { path: 'findlobby', component: FindlobbyComponent, canActivate: [OktaAuthGuard], data: {onAuthRequired: onAuthRequired}},
  { path: 'lobby/:id', component: LobbyComponent, canActivate: [OktaAuthGuard], data: {onAuthRequired: onAuthRequired}},
  { path: 'game/:id', component: GameComponent, canActivate: [OktaAuthGuard], data: {onAuthRequired: onAuthRequired}},
  { path: 'tissi', component: TissiComponent, canActivate: [OktaAuthGuard], data: {onAuthRequired: onAuthRequired}},
  { path: 'implicit/callback', component: OktaCallbackComponent },
  { path: '', component: HomeComponent}
];

const oktaConfig = Object.assign({
  onAuthRequired: ({oktaAuth, router}) => {
    console.log('router', router);
    router.navigate(['/login'])
  }
}, environment.oidc);

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [
    { provide: OKTA_CONFIG, useValue: environment.oidc }
  ]
})
export class AppRoutingModule { }
