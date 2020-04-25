import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: "root"
})
export class EmailService {
  constructor(private http: HttpClient) { }

  send(sender: string, message: string){
      return this.http.post('api/email/send', {
          "sender": sender,
          "message": message
      });
  }
  
}