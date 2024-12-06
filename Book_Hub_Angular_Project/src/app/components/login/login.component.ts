import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [CommonModule,FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  // user form model
 login = {
 
  email: '',
  password: ''
 
};





onSubmit() {
 
  console.log('Login Deytails:', this.login);
  
    localStorage.setItem('loginDetails', JSON.stringify(this.login));
 }
  
}
