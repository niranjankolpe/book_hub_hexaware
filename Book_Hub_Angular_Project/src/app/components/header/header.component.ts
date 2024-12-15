import { Component } from '@angular/core';

import { NavigationEnd, Router, RouterLink, RouterLinkActive } from '@angular/router';
import { LoginServicesService } from '../../services/login-services.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  userToken: any;
  displayUserInHeader: boolean = false;
  displayRegistrationInHeader: boolean = true;
  roleBasedDashboard: any;
  userRole: any;

  isNotificationDropdownOpen = false;

  constructor(private authService: LoginServicesService, private router: Router) {
    this.router.events.subscribe(event => {
      this.userToken = this.authService.getToken();
      if (this.userToken == null) {
        this.displayUserInHeader = false;
        this.displayRegistrationInHeader = true;
      }
      else {
        this.userToken = this.authService.decodeToken(this.userToken);
        this.displayUserInHeader = true;
        this.displayRegistrationInHeader = false;
        this.userRole = this.userToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

        if (this.userRole == "Administrator") {
          this.roleBasedDashboard = "/app-admin-dashboard";
        }
        else if (this.userRole == "Consumer") {
          this.roleBasedDashboard = "/app-user-dashboard";
        }
      }

      if (event instanceof NavigationEnd) {
        if (event.urlAfterRedirects === '/app-user-dashboard' && this.userRole != 'Consumer') {
          alert("Login with Consumer Account to access this page!");
          this.router.navigate([this.roleBasedDashboard]);
        }
        else if (event.urlAfterRedirects === '/app-admin-dashboard' && this.userRole != 'Administrator') {
          alert("Login with Administrator Account to access this page!");
          this.router.navigate([this.roleBasedDashboard]);
        }
        else if (event.urlAfterRedirects === '/app-login' && this.userToken != null) {
          this.router.navigate([this.roleBasedDashboard]);
        }
        else if (event.urlAfterRedirects === '/app-user-register-form' && this.userToken != null) {
          this.router.navigate([this.roleBasedDashboard]);
        }
      }
    });
  }

  toggleNotifications(){
    this.isNotificationDropdownOpen = !this.isNotificationDropdownOpen;
  }

  logout() {
    this.authService.logout().subscribe((result:any)=>{
      console.log("Logged out result: ",result);
    });
    this.authService.removeToken();
    this.router.navigate(["app-home"]);
    alert("Logged out successfully!");
    this.displayUserInHeader = false;
  }
}
