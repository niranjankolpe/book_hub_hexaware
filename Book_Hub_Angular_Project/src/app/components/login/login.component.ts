
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginServicesService } from '../../services/login-services.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;

  constructor(private authS: LoginServicesService, private router: Router, public http: HttpClient) {
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      Email: new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]),
      PasswordHash: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)])
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const Email = this.loginForm.get('Email')?.value;
      const PasswordHash = this.loginForm.get('PasswordHash')?.value;

      this.authS.login(Email, PasswordHash).subscribe({
        next: (token: string) => {
          this.authS.setToken(token);

          var decodedToken = this.authS.decodeToken(this.authS.getToken());
          var role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
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
        },
        error: (errorResponse) => {
          if (errorResponse.status === 400){
            alert(errorResponse.error);
          }
          else{
            alert("Some error occured");
            console.log(errorResponse.title);
          }
        },
      });
    }
    else {
      alert('Invalid Input Format. Please recheck!');
    }
  }
}
