import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-contact-us',
  imports: [FormsModule],
  templateUrl: './contact-us.component.html',
  styleUrl: './contact-us.component.css'
})
export class ContactUsComponent {
  query = {
    type: '',
    description: ''
  }

  onSubmit(){
    localStorage.setItem('queryRecord', JSON.stringify(this.query));
  }
}
