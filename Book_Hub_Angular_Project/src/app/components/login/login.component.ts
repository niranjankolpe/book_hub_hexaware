import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  login = {
    email: '',
    password: ''
  };

  onSubmit() {
    console.log('Login Details: ', this.login);
    localStorage.setItem('loginDetails', JSON.stringify(this.login));
  }
}
