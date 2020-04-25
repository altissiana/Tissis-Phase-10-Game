import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms'
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SignupComponent } from './signup/signup.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { StartlobbyComponent } from './startlobby/startlobby.component';
import { FindlobbyComponent } from './findlobby/findlobby.component';
import { TissiComponent } from './tissi/tissi.component';
import { OktaAuthModule, OktaConfig } from '@okta/okta-angular';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';
import { LobbyComponent } from './lobby/lobby.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { environment } from 'src/environments/environment';
import { BaseApiInterceptor } from './interceptors/baseApi.interceptor';
import { GameComponent } from './game/game.component';
import { CardComponent } from './card/card.component';
import { DragAndDropModule } from 'angular-draggable-droppable';
import { PhaseComponent } from './phase/phase.component';
import { CardCombinationComponent } from './card-combination/card-combination.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';
import { ScoreCardComponent } from './score-card/score-card.component';
import { MatDialogModule } from '@angular/material/dialog';
import { WildCardRunComponent } from './wild-card-run/wild-card-run.component';
import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
  declarations: [
    AppComponent,
    SignupComponent,
    HomeComponent,
    LoginComponent,
    DashboardComponent,
    StartlobbyComponent,
    FindlobbyComponent,
    TissiComponent,
    LobbyComponent,
    GameComponent,
    CardComponent,
    PhaseComponent,
    CardCombinationComponent,
    ScoreCardComponent,
    WildCardRunComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    AppRoutingModule,
    OktaAuthModule,
    FormsModule,
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    DragAndDropModule,
    MatGridListModule,
    MatCardModule,
    MatSnackBarModule,
    MatBottomSheetModule,
    MatDialogModule,
    DragDropModule
  ],
  entryComponents: [
    WildCardRunComponent
  ],
  providers: [
    { provide: 'BASE_API_URL', useValue: environment.apiUrl },
    { provide: HTTP_INTERCEPTORS, useClass: BaseApiInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
