import { Component, OnInit, signal, Signal } from '@angular/core';

import { CommonModule } from '@angular/common';

import { Constant } from '../../constants/constants';
import { GetAllGenreService } from '../../services/get-all-genre.service';
import { FormControl, FormGroup, } from '@angular/forms';
import { NgForm } from '@angular/forms';

import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent {

  bookExplorerForm: FormGroup;
  //AddBook
  book = {
    Isbn: '',
    Title: '',
    Author: '',
    Publication: '',
    PublishedDate: '',
    Edition: '',
    Language: '',
    Description: '',
    Cost: '',
    AvailableQuantity: '',
    TotalQuantity: '',
    GenreId: ''
  };
  //updatebook
  updatebook = {
    BookId: '',
    AvailableQuantity: '',
    TotalQuantity: ''

  };
  //RemoveBook
  removebook = {
    bookid: ''
  };


  displaySearchBox: boolean = false;
  displayBookList: boolean = false;
  displayGenreList: boolean = false;
  displayBorrowedList: boolean = false;
  displayReservationList: boolean = false;
  displayNotificationList: boolean = false;
  displayUserList: boolean = false;
  displayLogList: boolean = false;
  displayFineList: boolean = false;

  displayAddBookForm: boolean = false;
  displayUpdateBookForm: boolean = false;
  displayRemovekForm: boolean = false;

  genreList: any[] = [];
  borrowedList: any[] = [];
  fineList: any[] = [];
  userList: any[] = [];
  logList: any[] = [];
  notificationList: any[] = [];
  reservationList: any[] = [];

  // Addbookcomponent


  filterTypeSelected: string = '';

  bookListToDisplay: any[] = [];

  constructor(private httpClient: HttpClient) {
    this.GetAllBooks();
    this.GetAllGenres();
    this.GetAllBorrowed();
    this.GetAllFines();
    this.GetAllUsers();
    this.GetAllLogs();
    this.GetAllNotifications();
    this.GetAllReservations();

    this.bookExplorerForm = new FormGroup({
      searchInput: new FormControl('')
    });
  }

  ApplyFilter(filterType: string) {
    this.filterTypeSelected = filterType;

    if (filterType == 'all') {
      //this.displaySearchBox = false;
      this.GetAllBooks();
      this.displayBookList = true;
      this.displayGenreList = false;

      this.displayBorrowedList = false;
      this.displayReservationList = false;
      this.displayNotificationList = false;
      this.displayUserList = false;
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }

    else if (filterType == 'genre') {
      this.GetAllGenres();
      this.displayGenreList = true;
      this.displayBookList = false;
      this.displayBorrowedList = false;
      this.displayReservationList = false;
      this.displayNotificationList = false;
      this.displayUserList = false;
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'borrowed') {
      this.GetAllBorrowed();
      this.displayBorrowedList = true;
      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayReservationList = false;
      this.displayNotificationList = false;
      this.displayUserList
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'fines') {
      this.GetAllFines();
      this.displayFineList = true;
      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;
      this.displayReservationList = false;
      this.displayNotificationList = false;
      this.displayUserList = false;
      this.displayLogList = false;

      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'users') {
      this.GetAllUsers();
      this.displayUserList = true;

      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;
      this.displayReservationList = false;
      this.displayNotificationList = false;
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'logs') {
      this.GetAllLogs();
      this.displayLogList = true;

      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;
      this.displayReservationList = false;
      this.displayNotificationList = false;
      this.displayUserList = false;

      this.displayFineList = false;
      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'notifications') {

      this.GetAllNotifications();
      this.displayNotificationList = true;

      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;
      this.displayReservationList = false;

      this.displayUserList = false;
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'reservations') {
      this.GetAllReservations();
      this.displayReservationList = true;

      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;

      this.displayNotificationList = false;
      this.displayUserList = false;
      this.displayLogList = false;
      this.displayFineList = false;
      this.displayAddBookForm = false;
      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'addBook') {
      this.displayAddBookForm = true;

      this.displayReservationList = false;

      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;

      this.displayNotificationList = false;
      this.displayUserList = false;
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayUpdateBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'updateBook') {
      this.displayUpdateBookForm = true;

      this.displayReservationList = false;

      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;

      this.displayNotificationList = false;
      this.displayUserList = false;
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayAddBookForm = false;
      this.displayRemovekForm = false;
    }
    else if (filterType == 'removeBook') {
      this.displayRemovekForm = true;

      this.displayReservationList = false;

      this.displayGenreList = false;
      this.displayBookList = false;
      this.displayBorrowedList = false;

      this.displayNotificationList = false;
      this.displayUserList = false;
      this.displayLogList = false;
      this.displayFineList = false;

      this.displayUpdateBookForm = false;
      this.displayAddBookForm = false;
    }

    else {
      this.displaySearchBox = false;
      this.displayBookList = false;
    }
  }

  GetAllBooks() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_Books).subscribe((result: any) => {
      this.bookListToDisplay = result.$values;
    })
  }

  GetAllGenres() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_All_Genre).subscribe((result: any) => {
      this.genreList = result.$values;

    })
  }

  GetAllBorrowed() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_Borrowed).subscribe((result: any) => {
      this.borrowedList = result.$values;

    })

  }
  GetAllFines() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_Fines).subscribe((result: any) => {
      this.fineList = result.$values;

    })

  }
  GetAllUsers() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_Users).subscribe((result: any) => {
      this.userList = result.$values;


    })

  }
  GetAllLogs() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_Logs).subscribe((result: any) => {
      this.logList = result.$values;

    })

  }
  GetAllNotifications() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_Notifications).subscribe((result: any) => {
      this.notificationList = result.$values;
      console.log(result);
    })

  }
  GetAllReservations() {
    this.httpClient.get(Constant.BASE_URI + Constant.Get_Reservations).subscribe((result: any) => {
      this.reservationList = result.$values;
      console.log(result);
    })

  }


  // onSubmit(){
  //   if (this.filterTypeSelected == 'AddBook'){

  //     // const bookId = this.bookExplorerForm?.get('searchInput')?.value;
  //     // const formData = new FormData();
  //     // formData.append('bookId', bookId);
  //     // this.httpClient.post("https://localhost:7251/api/User/GetBookByBookId", formData).subscribe((result:any)=>{
  //     //   this.bookListToDisplay = [result.value];
  //     // });
  //     // this.displayBookList = true;



  // }

  //   else if (this.filterTypeSelected == 'isbn'){
  //     const isbn = this.bookExplorerForm?.get('searchInput')?.value;
  //     const formData = new FormData();
  //     formData.append('isbn', isbn);
  //     this.httpClient.post("https://localhost:7251/api/User/GetBookByISBN", formData).subscribe((result:any)=>{
  //       this.bookListToDisplay = [result.value];
  //     });
  //     this.displayBookList = true;
  //   }
  //   else if (this.filterTypeSelected == 'genre'){
  //     const genreId = this.bookExplorerForm?.get('searchInput')?.value;
  //     const formData = new FormData();
  //     formData.append('genreId', genreId);
  //     console.log(genreId);
  //     this.httpClient.post("https://localhost:7251/api/User/GetBooksByGenre", formData).subscribe((result:any)=>{
  //       console.log(result);
  //       this.bookListToDisplay = result.value.$values;
  //     });
  //     this.displayBookList = true;
  //   }
  //   else if (this.filterTypeSelected == 'author'){
  //     console.log("Book List to Display before Author:", this.bookListToDisplay, ", and is display true: ", this.displayBookList);
  //     const authorName = this.bookExplorerForm?.get('searchInput')?.value;
  //     const formData = new FormData();
  //     formData.append('authorName', authorName);
  //     console.log(authorName);
  //     this.httpClient.post("https://localhost:7251/api/User/GetBooksByAuthor", formData).subscribe((result:any)=>{
  //       console.log(result);
  //       this.bookListToDisplay = result.value.$values;
  //       console.log("Book List to Display after Author:", this.bookListToDisplay, ", and is display true: ", this.displayBookList);
  //     });
  //     this.displayBookList = true;
  //   }
  // }
  //***********************************************working new code******************************************************************* */
  //AddBookSubmit Correct Code is working in html 294 line
  onBookAdd(addbookForm: any): void {
    if (addbookForm.valid) {
      const apiUrl = 'https://localhost:7251/api/Admin/AddBook'; // Replace with your actual API URL

      // Create FormData to send form data as a traditional form submission
      const formData = new FormData();
      formData.append('Isbn', this.book.Isbn);
      formData.append('Title', this.book.Title);
      formData.append('Author', this.book.Author);
      formData.append('Publication', this.book.Publication);
      formData.append('PublishedDate', this.book.PublishedDate);
      formData.append('Edition', this.book.Edition);
      formData.append('Language', this.book.Language);
      formData.append('Description', this.book.Description);
      formData.append('Cost', this.book.Cost.toString());
      formData.append('AvailableQuantity', this.book.AvailableQuantity.toString());
      formData.append('TotalQuantity', this.book.TotalQuantity.toString());
      formData.append('GenreId', this.book.GenreId);

      // Send the POST request with FormData
      this.httpClient.post(apiUrl, formData).subscribe({
        next: (response) => {
          console.log('Book added successfully:', response);
          alert('Book added successfully!');
          addbookForm.resetForm(); // Reset the form
        },
        error: (error) => {
          console.error('Error adding book:', error);
          alert('An error occurred while adding the book.');
        },
      });
    } else {
      alert('Please fill out all required fields.');
    }
  }


  //UpdateBookForm  Correct Code is working in html 355 line

  onBookUpdate(updatebookForm: any): void {
    if (updatebookForm.valid) {
      const apiUrl = 'https://localhost:7251/api/Admin/Updatebook'; // Your API endpoint

      // Create a FormData object to match the expected format in the backend
      const formData = new FormData();
      formData.append('BookId', this.updatebook.BookId);
      formData.append('AvailableQuantity', this.updatebook.AvailableQuantity);
      formData.append('TotalQuantity', this.updatebook.TotalQuantity);

      // Send the PATCH request with FormData
      this.httpClient.patch(apiUrl, formData).subscribe({
        next: (response) => {
          console.log('Book updated successfully:', response);
          alert('Book updated successfully!');
          updatebookForm.resetForm(); // Reset the form after successful submission
        },
        error: (error) => {
          console.error('Error updating book:', error);
          alert('An error occurred while updating the book.');
        },
      });
    } else {
      alert('Please fill out all required fields.');
    }
  }

  //Remove book working code in html line 379 
  onBookRemove(removebookForm: NgForm) {
    if (removebookForm.valid) {
      const apiUrl = 'https://localhost:7251/api/Admin/RemoveBook'; // Backend API URL
      const formData = new FormData();
      formData.append('bookid', this.removebook.bookid);

      this.httpClient.patch(apiUrl, formData).subscribe({
        next: (response) => {
          console.log('Book removed successfully:', response);
          alert('Book removed successfully!');
          removebookForm.resetForm(); // Reset the form
        },
        error: (error) => {
          console.error('Error removing book:', error);
          alert('An error occurred while removing the book.');
        },
      });
    } else {
      alert('Please fill out the form correctly.');
    }
  }



}








