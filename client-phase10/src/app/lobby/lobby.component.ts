import { Component, OnInit, ViewChild, ElementRef, QueryList, ViewChildren } from '@angular/core';
import { OktaAuthService } from '@okta/okta-angular';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { LobbyService } from './lobby.service';
import { LobbyResourceModel } from './lobbyResourceModel';
import { HubConnection } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { PlayerResourceModel } from '../player/playerResourceModel';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.scss']
})
export class LobbyComponent implements OnInit {
  lobby: LobbyResourceModel;
  chatConnection: HubConnection;
  lobbyConnection: HubConnection;
  chats: string[];
  message: string;

  @ViewChild("chatbox") chatbox: ElementRef;
  @ViewChildren("messages") messages: QueryList<any>;


  constructor(public oktaAuth: OktaAuthService, private router: Router, private route: ActivatedRoute, private lobbyService: LobbyService) {
    this.lobby = { players: [] as PlayerResourceModel[]} as LobbyResourceModel;
    this.chats = [];
  }

  ngOnInit(): void {
    console.log('id', this.route.snapshot.paramMap.get('id'));
    const lobbyId = this.route.snapshot.paramMap.get('id');

    this.lobbyService.getById(lobbyId)
    .subscribe (result => {
      if (result === null) {
        this.router.navigate(['/dashboard'])
      } else {
        this.lobby = result; 

        this.establishChatConnection();
        this.establishLobbyConnection();
      }
    });
  }

  ngAfterViewInit() {
    this.messages.changes.subscribe(x => {
      this.scrollToBottom();
    });
  }

  establishChatConnection() {
    const baseUrl = environment.apiUrl;
    this.chatConnection = new signalR.HubConnectionBuilder().withUrl(`${baseUrl}/chatHub`).build();

    this.chatConnection.on("ReceiveMessage", data => {
      this.chats.push(`${data.user}: ${data.message}`);
    });

    this.chatConnection.start().then(() => {
      console.log('started chat connection');
      this.chatConnection.send("JoinLobby", this.lobby.id);
    })
  }

  establishLobbyConnection() {
    const baseUrl = environment.apiUrl;
    this.lobbyConnection = new signalR.HubConnectionBuilder().withUrl(`${baseUrl}/lobbyHub`).build();

    this.lobbyConnection.on("LobbyUpdate", data => {
      if (this.lobby.id === data.id) {
        this.lobby = data;
      }
    });

    this.lobbyConnection.on("StartGame", () => {
      console.log('received start game message');
      this.router.navigate(["/game", this.lobby.id]);
    });

    this.lobbyConnection.start().then(() => {
      console.log('started lobby connection');
      this.lobbyConnection.send("JoinLobby", this.lobby.id);
    })
  }

  sendMessage = async () => {
    const userClaims = await this.oktaAuth.getUser();
    const user = userClaims.given_name;
    
    this.chatConnection.send("SendMessage", this.lobby.id, user, this.message);
    this.message = '';
  }

  private scrollToBottom() {
    console.log('scrolling to bottom');
    console.log('chatbox', this.chatbox);
    this.chatbox.nativeElement.scrollTop = this.chatbox.nativeElement.scrollHeight;
  }

  startGame() {
    this.lobbyService.startGame(this.lobby.id)
      .subscribe(
        (data: any) => console.log("response"), 
        error => console.log("error", error)
      )
  }
}

