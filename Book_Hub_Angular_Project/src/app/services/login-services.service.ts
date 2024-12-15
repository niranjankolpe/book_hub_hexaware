import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Constant } from '../constants/constants';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})

export class LoginServicesService {

  constructor(private http:HttpClient) { }

  login(Email: string, PasswordHash: string): Observable<string> {
    const formData = new FormData();
    formData.append('Email', Email);
    formData.append('PasswordHash', PasswordHash);
    console.log(Email, PasswordHash);
  
    return this.http.post(Constant.BASE_URI + Constant.LOGIN, formData, {
      responseType: 'text', // Expect plain text
      observe: 'body',      // Observe the body only
    });
  }

  logout(){
    const formData = new FormData();
    const decodedToken = this.decodeToken(this.getToken());
    formData.append('userId', decodedToken["UserId"]);
    return this.http.post("https://localhost:7251/api/Home/Logout", formData);
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
  
  decodeToken(token:any): any | null {
    if (token) {
      return jwtDecode(token);
    }
    return null;
  }

  getTokenExpiry(token:any): Date | null {
    const decodedToken = this.decodeToken(token);
    if (decodedToken && decodedToken.exp) {
      const expiryDate = new Date(decodedToken.exp * 1000);
      return expiryDate;
    }
    return null;
  }
}
