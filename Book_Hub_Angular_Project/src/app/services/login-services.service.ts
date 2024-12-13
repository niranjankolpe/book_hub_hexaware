import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { of } from 'rxjs';
import { Constant } from '../constants/constants';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class LoginServicesService {

  constructor(private http:HttpClient) { 
   
  }

  // login(Email:string,PasswordHash:string): Observable<any>  {
  //   const formData = new FormData();
  //   formData.append('Email',Email);
  //   formData.append('PasswordHash',PasswordHash);
    
  //   //call the API
  //   //In Post Supply 1. URL and 2. Param
  //   return this.http.post(Constant.BASE_URI+Constant.LOGIN,formData);
  // }

  login(Email: string, PasswordHash: string): Observable<string> {
    const formData = new FormData();
    formData.append('Email', Email);
    formData.append('PasswordHash', PasswordHash);
  
    return this.http.post(Constant.BASE_URI + Constant.LOGIN, formData, {
      responseType: 'text', // Expect plain text
      observe: 'body',      // Observe the body only
    });
  }

  logout(){
    const formData = new FormData();
    const userId = this.decodeToken(this.getToken());

    formData.append('userId', userId["UserId"]);
  
    return this.http.post("https://localhost:7251/api/Home/Logout", formData).subscribe((result)=>{
      
      console.log("Logged Out Successfully!");
    });
  }
  
  getToken(): string | null  {
    return localStorage.getItem('token');
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  removeToken(): void {
    localStorage.removeItem('token');
  }
  // New method to decode the token and another method to extract tokens exp
  decodeToken(token:any): any | null {
   // const token = this.getToken();
    if (token) {
      try {
        return jwtDecode(token); // Decode the token and return its payload
      } catch (error) {
        console.error('Error decoding token:', error);
        return null;
      }
    }
    return null;
  }
  getTokenExpiry(token:any): Date | null {
    const decodedToken = this.decodeToken(token);
    if (decodedToken && decodedToken.exp) {
      // Convert the exp value from Unix timestamp to a Date object
      const expiryDate = new Date(decodedToken.exp * 1000); // Multiply by 1000 to convert seconds to milliseconds
      return expiryDate;
    }
    return null;
  }

}
