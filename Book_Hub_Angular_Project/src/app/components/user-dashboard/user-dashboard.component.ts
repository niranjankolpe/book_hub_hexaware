import { Component } from '@angular/core';
import { LoginServicesService } from '../../services/login-services.service';
import { ActivatedRoute, Router, RouterLink, RouterLinkActive } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Constant } from '../../constants/constants';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, FormsModule, Validators } from '@angular/forms';
import { OtpServiceService } from '../../services/otp-service.service';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-dashboard',
  imports: [CommonModule, RouterLink, RouterLinkActive, FormsModule, ReactiveFormsModule],
  templateUrl: './user-dashboard.component.html',
  styleUrl: './user-dashboard.component.css'
})
export class UserDashboardComponent {
  Notification_Type = Constant.Notification_Type;
  Borrow_Status = Constant.Borrow_Status;
  Reservation_Status = Constant.Reservation_Status;
  Fine_Type = Constant.Fine_Type;
  Fine_Paid_Status = Constant.Fine_Paid_Status;
  userToken: any;
  userRole: any;

  filterTypeSelected: any = '';
  settingsFilterType: any = '';

  displayNotificationList: boolean = false;
  notificationList: any[] = [];

  displayBorrowingList: boolean = false;
  borrowingList: any[] = [];

  displayReservationList: boolean = false;
  reservationList: any[] = [];

  displayFineList: boolean = false;
  fineList: any[] = [];

  displaySection: any = '0';

  displayUpdateAccountForm = false;
  displayUpdatePasswordForm = false;
  displayDeleteAccountForm = false;

  updatePasswordForm!: FormGroup;
  updateUserForm!: FormGroup;

  otpValidationForm: FormGroup;
  displayOTPForm: boolean = false;
  generatedOTP: any;

  constructor(private authService: LoginServicesService, private router: Router, private route: ActivatedRoute, private httpClient: HttpClient, private otpService: OtpServiceService) {
    this.userToken = this.authService.getToken();
    if (this.userToken == null) {
      this.router.navigate(["/app-login"]);
    }
    this.userToken = authService.decodeToken(this.userToken);
    this.userRole = this.userToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];


    this.GetUserNotifications();
    this.GetUserBorrowings();
    this.GetUserReservations();
    this.GetUserFines();

    this.updatePasswordForm = new FormGroup({
      oldPassword: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]),
      newPassword: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]),
    });

    this.updateUserForm = new FormGroup({
      name: new FormControl('', [Validators.pattern(/^[A-Za-z\s]+$/)]),
      phone: new FormControl('', [Validators.pattern(/^\d{10}$/)]),
      address: new FormControl('')
    });

    this.route.queryParams.subscribe((params) => {
      this.displaySection = params['displaySection'];// Convert to boolean if necessary
    });
    this.otpValidationForm = new FormGroup({
      otp: new FormControl('')
    });

    // if (this.userRole != "Consumer"){
    //   this.router.navigate(["/app-login"]);
    //   alert("Login with consumer account to access User Dashboard!");
    // }
  }

  ApplyFilter(filterType: string) {
    this.filterTypeSelected = filterType;

    if (filterType == 'notifications') {
      this.GetUserNotifications();
      this.displayNotificationList = true;

      this.displayBorrowingList = false;
      this.displayReservationList = false;
      this.displayFineList = false;
    }
    else if (filterType == 'borrowings') {
      this.GetUserBorrowings();
      this.displayBorrowingList = true;

      this.displayNotificationList = false;
      this.displayReservationList = false;
      this.displayFineList = false;
    }
    else if (filterType == 'reservations') {
      this.GetUserReservations();
      this.displayReservationList = true;

      this.displayNotificationList = false;
      this.displayBorrowingList = false;
      this.displayFineList = false;
    }
    else if (filterType == 'fines') {
      this.GetUserFines();
      this.displayFineList = true;

      this.displayNotificationList = false;
      this.displayBorrowingList = false;
      this.displayReservationList = false;
    }
  }

  ApplySettingsFilter(settingsFilterType: string) {
    this.settingsFilterType = settingsFilterType;

    if (settingsFilterType == 'updateAccount') {
      this.displayUpdateAccountForm = true;
      this.displayUpdatePasswordForm = false;
      this.displayDeleteAccountForm = false;
    }
    else if (settingsFilterType == 'resetPassword') {
      this.displayUpdatePasswordForm = true;
      this.displayUpdateAccountForm = false;
      this.displayDeleteAccountForm = false;
    }
    else if (settingsFilterType == 'deleteAccount') {
      this.displayDeleteAccountForm = true;
      this.displayUpdateAccountForm = false;
      this.displayUpdatePasswordForm = false;
    }
  }

  returnBook(borrowId: string) {
    const formData = new FormData();
    formData.append('borrowId', borrowId);
    this.httpClient.patch("https://localhost:7251/api/User/ReturnBook", formData).subscribe((result: any) => {
      console.log("Success: ", result);
      alert("Success");
    },
      (error: Error) => {
        console.log("Got error: ", error.message);
        alert("Hmm got some error");
      });
    this.displayBorrowingList = false;
  }

  reportLostBook(borrowId: string) {
    const formData = new FormData();
    formData.append('borrowId', borrowId);
    console.log("Borrow ID: ", borrowId);
    this.httpClient.patch("https://localhost:7251/api/User/ReportLostBook", formData).subscribe((result: any) => {
      console.log("Success: ", result);
      alert("Success");
    },
      (error: Error) => {
        console.log("Got error: ", error.message);
        alert("Hmm got some error");
      });
    this.displayBorrowingList = false;
  }

  cancelReservation(reservationId: string) {
    const formData = new FormData();
    formData.append('reservationId', reservationId);
    console.log("Reservation ID: ", reservationId);
    this.httpClient.patch("https://localhost:7251/api/User/CancelBookReservation", formData).subscribe((result: any) => {
      console.log("Success: ", result);
      alert("Success");
    },
      (error: Error) => {
        console.log("Got error: ", error.message);
        alert("Hmm got some error");
      });
    this.displayReservationList = false;
  }

  updateUser() {
    if (this.updateUserForm.valid) {
      const formData = new FormData();
      const name = this.updateUserForm.get("name")?.value;
      const phone = this.updateUserForm.get("phone")?.value;
      const address = this.updateUserForm.get("address")?.value;

      formData.append('Name', name);
      formData.append('Phone', phone);
      formData.append('Address', address);
      formData.append('UserId', this.userToken["UserId"]);

      console.log(this.userToken["UserId"]);
      console.log(name);
      console.log(phone);
      console.log(address);

      this.httpClient.patch("https://localhost:7251/api/Home/UpdateUser", formData).subscribe((result: any) => {
        console.log("Success: ", result);
        alert("Success");
      },
        (error: Error) => {
          console.log("Got error: ", error.message);
          alert("Hmm got some error");
        });

      this.router.navigate(['/app-home']);
    }
    else{
      alert("One or more input fields have incorrect format!");
    }
  }

  resetUserPassword() {
    if (this.updatePasswordForm.valid) {
      const formData = new FormData();
      const userId = this.userToken["UserId"];
      const email = this.userToken["Email"];
      const oldPassword = this.updatePasswordForm.get("oldPassword")?.value;
      const newPassword = this.updatePasswordForm.get("newPassword")?.value;
      formData.append('UserId', userId);
      formData.append('Email', email);
      console.log("Reset Password Old Password: ", oldPassword);
      console.log("Reset Password New Password: ", newPassword);
      formData.append('OldPassword', oldPassword);
      formData.append('NewPassword', newPassword);

      this.httpClient.patch("https://localhost:7251/api/User/ResetPassword", formData).subscribe((result: any) => {
        console.log("Success: ", result);
        alert("Success");
      },
        (error: Error) => {
          console.log("Got error: ", error.message);
          alert("Hmm got some error");
        });

      this.router.navigate(['/app-home']);
    }
    else {
      alert("Passwords should have length 8 to 20 characters");
    }
  }

  deleteUser() {
    confirm("This will delete your entire account! Confirm?");
    this.displayDeleteAccountForm = false;
    this.displayOTPForm = true;
    const formData = new FormData();
    const userId = this.userToken["UserId"];
    const emailAddress = this.userToken["Email"];
    formData.append('userId', userId);

    this.otpService.sendDeleteOTP(emailAddress);
    console.log("User ID: ", userId);
  }

  validateDeleteOTP() {
    const otp = this.otpValidationForm.get('otp')?.value;
    var result = this.otpService.validateOTP(otp);
    if (result == true) {
      const formData = new FormData();
      const userId = this.userToken["UserId"];
      formData.append('userId', userId);
      console.log("Sending delete request...........");
      this.httpClient.post("https://localhost:7251/api/Home/DeleteUser", formData).subscribe((result: any) => {
        this.authService.removeToken();
        console.log("Success: ", result);
        alert("Account deleted successfully!");
      },
        (error: any) => {
          alert(error.error);
        });
      this.router.navigate(["/app-home"]);
    }
    else{
      alert("OTP does not match try again");
    }
  }

  GetUserNotifications() {
    const userId = this.userToken["UserId"];
    const formData = new FormData();
    formData.append("userId", userId);
    this.httpClient.post("https://localhost:7251/api/User/GetNotificationsByUserId", formData).subscribe((result: any) => {
      this.notificationList = result.value.$values;
      console.log("Notifications by User: ", result);
      console.log("Notifications by User: ", result.value.$values);
    });
  }

  GetUserBorrowings() {
    const userId = this.userToken["UserId"];
    const formData = new FormData();
    formData.append("userId", userId);
    this.httpClient.post("https://localhost:7251/api/User/GetBorrowedByUserId", formData).subscribe((result: any) => {
      this.borrowingList = result.value.$values;
      console.log(result);
    });
  }

  GetUserReservations() {
    const userId = this.userToken["UserId"];
    const formData = new FormData();
    formData.append("userId", userId);
    this.httpClient.post("https://localhost:7251/api/User/GetReservationsByUserId", formData).subscribe((result: any) => {
      this.reservationList = result.value.$values;
      console.log(result);
    });
  }

  GetUserFines() {
    this.GetUserBorrowings();
    this.borrowingList.forEach((borrowed) => {
      this.fineList = this.fineList.concat(borrowed.fines.$values);
      console.log(borrowed.fines.$values);
    })
  }
}
