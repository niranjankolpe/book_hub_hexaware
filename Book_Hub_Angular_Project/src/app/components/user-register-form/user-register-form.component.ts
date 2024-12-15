import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { OtpServiceService } from '../../services/otp-service.service';

@Component({
  selector: 'app-user-register-form',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './user-register-form.component.html',
  styleUrl: './user-register-form.component.css',
})
export class UserRegisterFormComponent {
  registrationForm!: FormGroup;
  otpValidationForm: FormGroup;

  displayOTPForm: boolean = false;

  constructor(private router: Router, private otpService: OtpServiceService) {
    this.registrationForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)]),
      email: new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]),
      phone: new FormControl('', [Validators.required, Validators.pattern(/^[-+]?\d+$/), Validators.minLength(10), Validators.maxLength(10)]),
      address: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]),
      confirmPassword: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)])
    });

    this.otpValidationForm = new FormGroup({
      otp: new FormControl('')
    });
  }

  onSubmit() {
    if (this.registrationForm.valid){
      const password = this.registrationForm.get('password')?.value;
      const confirmPassword = this.registrationForm.get('confirmPassword')?.value;
      if (password !== confirmPassword) {
        alert("Passwords do not match!");
      }
      else {
        const dataToSubmit = new FormData();
        const name = this.registrationForm.get('name')?.value;
        const email = this.registrationForm.get('email')?.value;
        const phone = this.registrationForm.get('phone')?.value;
        const address = this.registrationForm.get('address')?.value;
        
        dataToSubmit.append('Name', name);
        dataToSubmit.append('Email', email);
        dataToSubmit.append('Phone', phone);
        dataToSubmit.append('Address', address);
        dataToSubmit.append('PasswordHash', password);
        this.otpService.sendOTP(dataToSubmit);
        this.displayOTPForm = !this.displayOTPForm;
      }
    }
    else{
      alert("One or more inputs have invalid format. Please recheck!")
    }
  }

  validateOTP() {
    const otp = this.otpValidationForm.get('otp')?.value;
    var result = this.otpService.validateOTP(otp);
    if (result == true) {
      this.otpService.createUser();
      this.router.navigate(["/app-login"]);
    }
    else{
      alert("OTP does not match, try again!");
    }
  }
}
