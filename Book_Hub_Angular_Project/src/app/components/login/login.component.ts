
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginServicesService } from '../../services/login-services.service';
import { Router } from '@angular/router';
import { Constant } from '../../constants/constants';
import { HttpClient } from '@angular/common/http';

import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink, RouterLinkActive],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;
  constructor(private authS: LoginServicesService, private router: Router, public http: HttpClient) { }
  ngOnInit(): void {
    this.loginForm = new FormGroup({
      Email: new FormControl('', Validators.required),
      PasswordHash: new FormControl('', Validators.required)
    });
  }

  // onSubmit():void{
  //   const Email=this.loginForm?.get('Email')?.value;
  //   const PasswordHash = this.loginForm?.get('PasswordHash')?.value;
  //   let token:any;
  //   const formData = new FormData();
  //   formData.append('Email',Email);
  //   formData.append('PasswordHash',PasswordHash);

  //   //call the API
  //   //In Post Supply 1. URL and 2. Param
  //    this.http.post(Constant.BASE_URI+Constant.LOGIN,formData).subscribe((result)=>{
  //     token = result;
  //    })
  //   console.log("token is " ,token);
  // //   this.authS.setToken(token);
  // //  console.log( this.decodeToken(token));
  // //   console.log("Token Expiry Date: ", this.authS.getTokenExpiry(token));


  // }
  onSubmit(): void {
    if (this.loginForm.valid) {
      const Email = this.loginForm.get('Email')?.value;
      const PasswordHash = this.loginForm.get('PasswordHash')?.value;

      this.authS.login(Email, PasswordHash).subscribe({
        next: (token: string) => {
          console.log('Token received:', token);
          this.authS.setToken(token); // Store token in localStorage
          //commonted morining decoded
          var decodedToken = this.authS.decodeToken(token);
          console.log("Decoded Token: ", decodedToken);


          // console.log('Decoded token:', this.decodeToken(token));
          const expiryDate = this.authS.getTokenExpiry(token);
          console.log('Token expiry date:', expiryDate);

          var role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
          console.log("Role: ", role);
          if (role == "Administrator") {
            this.router.navigate(['/app-admin-dashboard']);
          }
          else if (role == "Consumer") {
            this.router.navigate(['/app-user-dashboard']);
          }
          else {
            this.router.navigate(['/app-home']);
            alert("Something went wrong while login!");
          }
          //this.router.navigate(['/adashboard']); // Navigate to another route on success
        },
        error: (err) => {
          console.error('Login failed:', err);
          alert('Login failed. Please check your credentials. If creds are ok, the problem is on our side');
        },
      });
    } else {
      alert('Please fill in the required fields.');
    }
  }


  decodeToken(token: any) {
    const decodedToken = this.authS.decodeToken(token);
    if (decodedToken) {
      console.log('Decoded Token:', decodedToken);
      // Now you can use the information in the token
    } else {
      console.log('No token found or unable to decode');
    }
  }
}
