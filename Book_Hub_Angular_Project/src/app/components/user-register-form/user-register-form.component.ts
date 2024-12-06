import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-register-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-register-form.component.html',
  styleUrl: './user-register-form.component.css'
})
export class UserRegisterFormComponent {
  // user form model
  user = {
    name: '',
    email: '',
    phone: '',
    address: '',
    password: '',
    confirmpassword: ''
  };

  fetcheduser: any = null;

  onSubmit() {
    if (this.user.password !== this.user.confirmpassword){
      confirm("Passwords do not match!");
    }
    else{
      confirm("Details saved successfully!");
      location.replace('/app-home')
      console.log('User Details:', this.user);
      localStorage.setItem('UserDetails', JSON.stringify(this.user));
    }
  }

  fetchData() {
    const storedEmployee = JSON.parse(localStorage.getItem('UserDetails') || '{}');
    this.fetcheduser = storedEmployee;
    console.log('Fetched User Data:', this.fetcheduser);
  }
}
