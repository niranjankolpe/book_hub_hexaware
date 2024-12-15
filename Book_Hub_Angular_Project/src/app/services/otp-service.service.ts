import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
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
    console.log("Sending otp for email: ", formD.get("emailAddress"));
    this.httpClient.post("https://localhost:7251/api/Home/GenerateOTP", formD).subscribe((result: any) => {
      console.log("Got OTP value: ", result.value);
      this.generatedOTP = result.value;
      console.log("Got Generated value: ", this.generatedOTP);
    });

    // console.log("Email to send: ", this.userFormData.get("Email"));
    // this.generatedOTP = "randomstring";
  }

  sendForgotPassOTP(emailAddress: string) {
    var formData = new FormData();
    formData.append('emailAddress', emailAddress);
    console.log("Have email address:", emailAddress);
    this.httpClient.post("https://localhost:7251/api/Home/GenerateOTP", formData).subscribe((result: any) => {
      this.forgotPasswordOTP = result.value;
      console.log("Forgot Password OTP from API: ", this.forgotPasswordOTP);
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

    var genVal;
    this.httpClient.post("https://localhost:7251/api/Home/GenerateOTP", formD).subscribe((result: any) => {
      genVal = result.value;
      console.log("Got OTP value: ", result.value);
      this.generatedOTP = genVal;
      console.log("Got temp gen val: ", genVal);
      console.log("Got Generated value: ", this.generatedOTP);
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
    this.httpClient.post("https://localhost:7251/api/Home/CreateUser", this.userFormData).subscribe((result: any) => {
      console.log("Got a response from Create/Register API as: ", result);
      alert("Success");
    });
    this.router.navigate(["/app-login"]);
  }
}
