import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

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

  constructor(private httpClient:HttpClient, private router: Router){

  }
  

  fetcheduser: any = null;

  onSubmit() {
    if (this.user.password !== this.user.confirmpassword){
      confirm("Passwords do not match!");
    }
    else{
      // confirm("Details saved successfully!");
      // location.replace('/app-home')
      // console.log('User Details:', this.user);
      // localStorage.setItem('UserDetails', JSON.stringify(this.user));

      // Proceed with API Endpoint and data submission

      // FromForm
      const dataToSubmit = new FormData(); 
      dataToSubmit.append('Name', this.user.name);
      dataToSubmit.append('Email', this.user.email);
      dataToSubmit.append('Phone', this.user.phone);
      dataToSubmit.append('Address', this.user.address);
      dataToSubmit.append('PasswordHash', this.user.password);

      // FromBody
      // const dataToSubmit = {
      //   'Name': this.user.name,
      //   'Email': this.user.email,
      //   'Phone': this.user.phone,
      //   'Address': this.user.address,
      //   'PasswordHash': this.user.password
      // };
      console.log(this.user.name);
      console.log(this.user.email);
      console.log(this.user.phone);
      console.log(this.user.address);
      console.log(this.user.password);
      this.httpClient.post("https://localhost:7251/api/Home/CreateUser", dataToSubmit).subscribe((result:any)=>{
        console.log("Got a response from Create/Register API as: ", result);
        alert("Success");
      });
      this.router.navigate(["/app-login"]);
    }
  }

  fetchData() {
    const storedEmployee = JSON.parse(localStorage.getItem('UserDetails') || '{}');
    this.fetcheduser = storedEmployee;
    console.log('Fetched User Data:', this.fetcheduser);
  }
}
