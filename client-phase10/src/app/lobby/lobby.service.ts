import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LobbyResourceModel } from './lobbyResourceModel';

@Injectable({
    providedIn: "root"
})
export class LobbyService {
  constructor(private http: HttpClient) { }

  create(name: string, size: number, playerName: string, playerId: string){
    
    return this.http.post<LobbyResourceModel>('api/lobby', {
        "name": name,
        "size": size,
        "playerName": playerName,
        "playerId": playerId
    });
  }

  get() {
    return this.http.get<LobbyResourceModel[]>('api/lobby');
  }

  getById(lobbyId: string) {
    return this.http.get<LobbyResourceModel>(`api/lobby/${lobbyId}`);
  } 

  joinLobby(lobbyId: string, userName: string, email: string) {
    return this.http.put<LobbyResourceModel>(`api/lobby/${lobbyId}`, {
      "userName": userName,
      "email": email
    });
  }

  startGame(lobbyId: string) {
    console.log("start game")
    return this.http.post<any>(`api/lobby/${lobbyId}/start`, {});
  }
  
}