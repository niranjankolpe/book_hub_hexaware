import { Component } from '@angular/core';

import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  picture ='https://www.edigitallibrary.com/img/library-img.jpg';
  title = 'Welcome to BookHub';
  description = 'Your gateway to a world of knowledge. Manage and explore an extensive collection of books.';
}
