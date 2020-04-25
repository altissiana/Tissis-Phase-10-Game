import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {

  signupForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    email: new FormControl(''),
    password: new FormControl('')
  });

  constructor(private accountService: AccountService, private router: Router){}

  onSubmit(){
    console.log("my form", this.signupForm.value);
    let firstName = this.signupForm.value.firstName;
    let lastName = this.signupForm.value.lastName;
    let email = this.signupForm.value.email;
    let password = this.signupForm.value.password;

    this.accountService.signup(firstName, lastName, email, password)
      .subscribe(response => {
        if (response.signupResponseType === 1) {
          this.router.navigate(["/login"]);
        }
      })
  }

}
