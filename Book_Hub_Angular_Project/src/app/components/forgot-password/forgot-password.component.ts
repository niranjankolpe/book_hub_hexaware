import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { OtpServiceService } from '../../services/otp-service.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { LoginServicesService } from '../../services/login-services.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  otpValidationForm: FormGroup;

  displayOTPForm: any = false;
  generatedOTP: any;

  constructor(private otpService: OtpServiceService, private httpClient: HttpClient, private authService: LoginServicesService, private router: Router) {
    this.forgotPasswordForm = new FormGroup({
      emailAddress: new FormControl('')
    });
    this.otpValidationForm = new FormGroup({
      otp: new FormControl(''),
      newPassword: new FormControl('')
    });
  }

  sendForgotPassOTP() {
    this.displayOTPForm = !this.displayOTPForm;
    var formData = new FormData();
    const emailAddress = this.forgotPasswordForm.get('emailAddress')?.value;
    formData.append("emailAddress", emailAddress);

    this.otpService.sendForgotPassOTP(emailAddress);
  }

  validateForgotPassOTP() {
    const otp = this.otpValidationForm.get('otp')?.value;
    var result = this.otpService.validateForgotPassOTP(otp);
    if (result == true) {
      var formData = new FormData();
      const emailAddress = this.forgotPasswordForm.get('emailAddress')?.value;
      const newPassword = this.otpValidationForm.get('newPassword')?.value;
      formData.append("emailAddress", emailAddress);
      formData.append("newPassword", newPassword);
      this.httpClient.post("https://localhost:7251/api/Home/ForgotPassword", formData).subscribe((result: any) => {
        console.log("Success: ", result);
        alert("Password updated successfully!");
      },
        (error: Error) => {
          console.log("Got error: ", error.message);
          alert("Hmm got some error");
        });
      this.router.navigate(["/app-login"]);
    }
    else {
      alert("Invalid otp entered!");
    }
  }
}
