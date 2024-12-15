import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-contact-us',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './contact-us.component.html',
  styleUrl: './contact-us.component.css'
})
export class ContactUsComponent {
  contactUsForm!: FormGroup;

  constructor(private httpClient: HttpClient, private router:Router) {
    this.contactUsForm = new FormGroup({
      emailAddress: new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+(?:\.[a-zA-Z0-9._%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]),
      queryType: new FormControl('', [Validators.required]),
      queryDescription: new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9\s.,]*$/)])
    });
  }

  onSubmit() {
    if (this.contactUsForm.valid) {
      const emailAddress = this.contactUsForm.get("emailAddress")?.value;
      const queryType = this.contactUsForm.get("queryType")?.value;
      const queryDescription = this.contactUsForm.get("queryDescription")?.value;

      const formData = new FormData();
      formData.append("Email", emailAddress);
      formData.append("Query_Type", queryType);
      formData.append("Description", queryDescription);

      console.log(emailAddress);
      console.log(queryType);
      console.log(queryDescription);

      this.httpClient.post("https://localhost:7251/api/Home/AddContactUsQuery", formData).subscribe((response:any) => {
        this.router.navigate(["/app-home"]);
        alert(response.value);
      },
      (error) => {
        alert("Something went wrong! We are looking into it.");
      });
    }
    else {
      alert("Please check email format. Also, the description box allows only text, numbers, comma and fullstop as input. No special characters allowed.");
    }
  }
}
