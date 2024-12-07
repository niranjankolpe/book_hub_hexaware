import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-explore-books',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './explore-books.component.html',
  styleUrl: './explore-books.component.css'
})

export class ExploreBooksComponent {
  bookExplorerForm: FormGroup;

  displaySearchBox:boolean = false;
  displayBookList:boolean = false;

  genreList:any[] = [];
  authorList:any[] = [];

  filterTypeSelected:string = '';

  bookListToDisplay:any[] = [];
  
  constructor(private httpClient: HttpClient){
    this.GetAllBooks();
    this.GetAllGenres();
    this.GetAllAuthors();
    this.bookExplorerForm = new FormGroup({
      searchInput: new FormControl('')
    });
  }

  ApplyFilter(filterType:string){
    this.filterTypeSelected = filterType;

    if (filterType == 'all'){
      this.displaySearchBox = false;
      this.GetAllBooks();
      this.displayBookList = true;
    }
    else if(filterType == 'bookId'){
      this.displaySearchBox = true;
      this.displayBookList = false;
    }
    else if(filterType == 'isbn'){
      this.displaySearchBox = true;
      this.displayBookList = false;
    }
    else if(filterType == 'genre'){
      this.displaySearchBox = true;
      this.displayBookList = false;
      this.GetAllGenres();
    }
    else if(filterType == 'author'){
      this.displaySearchBox = true;
      this.displayBookList = false;
      this.GetAllAuthors();
    }
    else{
      this.displaySearchBox = false;
      this.displayBookList = false;
    }
  }

  GetAllBooks(){
    this.httpClient.get("https://localhost:7251/api/Home/GetAllBooks").subscribe((result:any) => {
      this.bookListToDisplay = result.$values;
    })
  }

  GetAllGenres(){
    this.httpClient.get("https://localhost:7251/api/Admin/GetGenre").subscribe((result:any) => {
      this.genreList = result.$values;
    })
  }

  GetAllAuthors(){
    this.GetAllBooks();
    this.authorList = [];
    this.bookListToDisplay.forEach(element => {
      this.authorList.push(element.author);
    });
  }

  onSubmit(){
    if (this.filterTypeSelected == 'bookId'){
      const bookId = this.bookExplorerForm?.get('searchInput')?.value;
      const formData = new FormData();
      formData.append('bookId', bookId);
      this.httpClient.post("https://localhost:7251/api/User/GetBookByBookId", formData).subscribe((result:any)=>{
        this.bookListToDisplay = [result.value];
      });
      this.displayBookList = true;
    }
    else if (this.filterTypeSelected == 'isbn'){
      const isbn = this.bookExplorerForm?.get('searchInput')?.value;
      const formData = new FormData();
      formData.append('isbn', isbn);
      this.httpClient.post("https://localhost:7251/api/User/GetBookByISBN", formData).subscribe((result:any)=>{
        this.bookListToDisplay = [result.value];
      });
      this.displayBookList = true;
    }
    else if (this.filterTypeSelected == 'genre'){
      const genreId = this.bookExplorerForm?.get('searchInput')?.value;
      const formData = new FormData();
      formData.append('genreId', genreId);
      console.log(genreId);
      this.httpClient.post("https://localhost:7251/api/User/GetBooksByGenre", formData).subscribe((result:any)=>{
        console.log(result);
        this.bookListToDisplay = result.value.$values;
      });
      this.displayBookList = true;
    }
    else if (this.filterTypeSelected == 'author'){
      console.log("Book List to Display before Author:", this.bookListToDisplay, ", and is display true: ", this.displayBookList);
      const authorName = this.bookExplorerForm?.get('searchInput')?.value;
      const formData = new FormData();
      formData.append('authorName', authorName);
      console.log(authorName);
      this.httpClient.post("https://localhost:7251/api/User/GetBooksByAuthor", formData).subscribe((result:any)=>{
        console.log(result);
        this.bookListToDisplay = result.value.$values;
        console.log("Book List to Display after Author:", this.bookListToDisplay, ", and is display true: ", this.displayBookList);
      });
      this.displayBookList = true;
    }
  }
}
