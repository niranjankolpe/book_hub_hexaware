import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class OtpServiceService {
  otpValidationForm: FormGroup;

  userFormData: any;
  generatedOTP: any;

  forgotPasswordOTP: any;

  constructor(private httpClient: HttpClient, private router: Router) {
    this.otpValidationForm = new FormGroup({
      otp: new FormControl('')
    });
  }

  sendOTP(formData: FormData) {
    this.userFormData = formData;
    const formD = new FormData();
    formD.append("emailAddress", this.userFormData.get("Email"));
    this.httpClient.post("https://localhost:7251/api/Home/GenerateOTP", formD).subscribe((result: any) => {
      this.generatedOTP = result.value;
      alert("Enter the OTP sent to your Email");
    },
    ()=>{
      alert("An unexpected error occured on our side! Our team is notified an looking into it.");
      this.router.navigate(["/app-home"]);
    });
  }

  sendForgotPassOTP(emailAddress: string) {
    var formData = new FormData();
    formData.append('emailAddress', emailAddress);
    this.httpClient.post("https://localhost:7251/api/Home/GenerateOTP", formData).subscribe((result: any) => {
      this.forgotPasswordOTP = result.value;
    },
    ()=>{
      alert("An unexpected error occured on our side! Our team is notified an looking into it.");
      this.router.navigate(["/app-home"]);
    });
  }

  validateForgotPassOTP(otp: any) {
    if (this.forgotPasswordOTP == otp) {
      return true;
    }
    else {
      return false;
    }
  }

  sendDeleteOTP(emailAddress: any) {
    const formD = new FormData();
    formD.append("emailAddress", emailAddress);
    this.httpClient.post("https://localhost:7251/api/Home/GenerateOTP", formD).subscribe((result: any) => {
      this.generatedOTP = result.value;
    },
    ()=>{
      alert("An unexpected error occured on our side! Our team is notified an looking into it.");
      this.router.navigate(["/app-home"]);
    });
  }

  validateOTP(otp: any): boolean {
    if (this.generatedOTP == otp) {
      return true;
    }
    else {
      return false;
    }
  }

  createUser() {
    this.httpClient.post("https://localhost:7251/api/Home/CreateUser", this.userFormData).subscribe(() => {
      alert("Account Created Successfully! Please proceed to login.");
    },
    ()=>{
      alert("An unexpected error occured on our side! Our team is notified an looking into it.");
      this.router.navigate(["/app-home"]);
    });
    this.router.navigate(["/app-login"]);
  }
}
