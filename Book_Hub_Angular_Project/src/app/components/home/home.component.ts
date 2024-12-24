import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  picture ='https://www.edigitallibrary.com/img/library-img.jpg';
  title = 'Welcome to BookHub';
  description = 'Your gateway to a world of knowledge. Manage and explore an extensive collection of books.';

  constructor(private httpClient:HttpClient){

  }

  SendRandomEmail(){
    console.log("Activated function: SendRandomEmail()");
    var formData = new FormData();
    formData.append("emailAddress", "apurva.singhal.977@gmail.com");
    this.httpClient.post("https://localhost:7251/api/User/SendRandomEmail", formData).subscribe((result:any)=>{
      console.log("Got response from Email API Endpoint as : ", result);
    })
  }
}
