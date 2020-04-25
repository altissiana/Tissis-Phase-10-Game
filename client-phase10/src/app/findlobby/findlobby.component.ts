import { Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { LobbyResourceModel } from '../lobby/lobbyResourceModel';
import { OktaAuthService } from '@okta/okta-angular';
import { Router } from '@angular/router';
import { LobbyService } from '../lobby/lobby.service';
import { ToastrService } from 'ngx-toastr';
import { HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';



@Component({
  selector: 'app-findlobby',
  templateUrl: './findlobby.component.html',
  styleUrls: ['./findlobby.component.css']
})
export class FindlobbyComponent implements OnInit {

  searchModel: string;

  lobbies: LobbyResourceModel[];
  filteredLobbies: LobbyResourceModel[];
  hubConnection: HubConnection;

  constructor(public oktaAuth: OktaAuthService, 
    private router: Router, 
    private lobbyService: LobbyService, 
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.lobbyService.get()
    .subscribe(response => {
      this.lobbies = response;
      this.filteredLobbies = this.lobbies.filter(lobby => true);

      const baseUrl = environment.apiUrl;

      this.hubConnection = new signalR.HubConnectionBuilder().withUrl(`${baseUrl}/lobbyHub`).build();

      console.log('connection', this.hubConnection);

      this.hubConnection.on("LobbyUpdate", data => {
        let foundLobbyIndex = this.lobbies.findIndex(lobby => lobby.id === data.id);

        if(foundLobbyIndex >= 0) {
          this.lobbies[foundLobbyIndex] = data;
        } else {
          this.lobbies.push(data);
        }
      });

      this.hubConnection.start().then(() => {
        console.log('started hub connection');
      });
    });
  }

  async onLobbyClicked(lobbyId: string){
    const userClaims = await this.oktaAuth.getUser();
    const userName = userClaims.given_name;
    const email = userClaims.email;

    this.lobbyService.joinLobby(lobbyId, userName, email)
    .subscribe(result => {
      if (result === null) {
        this.toastr.error('Lobby no longer available');
      } else {
        let foundPlayer = result.players.find(player => player.id === email);

        if (!foundPlayer) {
          this.toastr.error('Lobby already full. Please select another lobby');
        } else {
          this.router.navigate(['/lobby', lobbyId]);
        }
      }
    });
  }

  creatorName(lobby: LobbyResourceModel) {
    return lobby.players.find(player => player.id === lobby.creatorId).name;
  }

  
  updateSearchModel(value: string) {
    console.log("search", value)
    value = value.toUpperCase();

    this.filteredLobbies = this.lobbies.filter(lobby =>
      lobby.name.toUpperCase().includes(value) || this.creatorName(lobby).toUpperCase().includes(value));
  }
}
