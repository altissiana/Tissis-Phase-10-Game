import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignupResponse } from './signupResponse';

@Injectable({
    providedIn: "root"
})
export class AccountService {
  constructor(private http: HttpClient) { }

  signup(firstName: string, lastName: string, email: string, password: string){
    
    return this.http.post<SignupResponse>('api/account/signup', {
        "firstName": firstName,
        "lastName": lastName,
        "email": email,
        "password": password
    });
  }
}

