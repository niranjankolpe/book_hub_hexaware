import { Component, OnInit, signal, Signal } from '@angular/core';

import { CommonModule } from '@angular/common';

import { Constant } from '../../constants/constants';
import { GetAllGenreService } from '../../services/get-all-genre.service';
import { FormControl, FormGroup, Validators, } from '@angular/forms';
import { NgForm } from '@angular/forms';

import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginServicesService } from '../../services/login-services.service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})

export class AdminDashboardComponent {
  Fine_Type = Constant.Fine_Type;
  Fine_Paid_Status = Constant.Fine_Paid_Status;
  User_Role = Constant.User_Role;
  Notification_Type = Constant.Notification_Type;
  Action_Type = Constant.Action_Type;
  Reservation_Status = Constant.Reservation_Status;
  Contact_Us_Query_Type = Constant.Contact_Us_Query_Type;
  Contact_Us_Query_Status = Constant.Contact_Us_Query_Status;
  Borrow_Status = Constant.Borrow_Status;

  adminEmail: any;

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
  displayContactUsQueries: boolean = false;

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
  consumerQueriesList: any[] = [];

  // Addbookcomponent


  filterTypeSelected: string = '';

  bookListToDisplay: any[] = [];

  constructor(private httpClient: HttpClient, private router: Router, private authService: LoginServicesService) {
    this.adminEmail = this.authService.decodeToken(this.authService.getToken())["Email"];

    this.GetAllBooks();
    this.GetAllGenres();
    this.GetAllBorrowed();
    this.GetAllFines();
    this.GetAllUsers();
    this.GetAllLogs();
    this.GetAllNotifications();
    this.GetAllReservations();
    this.GetAllConsumerQueries();

    this.bookExplorerForm = new FormGroup({
      searchInput: new FormControl('')
    });

    this.addbookForm = new FormGroup({
      isbn: new FormControl('', Validators.required),
      title: new FormControl('', [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)]),
      author: new FormControl('', [Validators.pattern(/^[A-Za-z\s]+$/)]),
      publication: new FormControl('', [Validators.pattern(/^[A-Za-z\s]+$/)]),
      publishedDate: new FormControl(''),
      edition: new FormControl(''),
      language: new FormControl('', [Validators.pattern(/^[A-Za-z\s]+$/)]),
      description: new FormControl(''),
      cost: new FormControl('', Validators.required),
      availableQuantity: new FormControl('', Validators.required),
      totalQuantity: new FormControl('', Validators.required),
      genreId: new FormControl('', Validators.required),
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
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
      this.displayContactUsQueries = false;
    }
    else if (filterType == 'consumerQueries') {
      this.displayContactUsQueries = true;

      this.displayRemovekForm = false;

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

  GetAllConsumerQueries() {
    this.httpClient.get("https://localhost:7251/api/Admin/GetAllConsumerQueries").subscribe((result: any) => {
      this.consumerQueriesList = result.$values;
      console.log(result);
    })
  }

  acknowledgeConsumerQuery(queryId: string) {
    const formData = new FormData();
    formData.append("queryId", queryId);
    this.httpClient.post("https://localhost:7251/api/Admin/AcknowledgeConsumerQuery", formData).subscribe((result: any) => {
      alert("Success!");
      this.router.navigate(["/app-home"]);
    },
      (error) => {
        alert("Some error occured!");
      });
  }
  addbookForm!: FormGroup;
  onBookAdd(): void {
    if (this.addbookForm.valid) {
      const apiUrl = 'https://localhost:7251/api/Admin/AddBook'; // Replace with your actual API URL

      // Create FormData to send form data as a traditional form submission
      const formData = new FormData();
      const isbn = this.addbookForm.get("isbn")?.value;
      const title = this.addbookForm.get("title")?.value;
      const author = this.addbookForm.get("author")?.value;
      const publication = this.addbookForm.get("publication")?.value;
      const publishedDate = this.addbookForm.get("publishedDate")?.value;
      const edition = this.addbookForm.get("edition")?.value;
      const language = this.addbookForm.get("language")?.value;
      const description = this.addbookForm.get("description")?.value;
      const cost = this.addbookForm.get("cost")?.value;
      const availableQuantity = this.addbookForm.get("availableQuantity")?.value;
      const totalQuantity = this.addbookForm.get("totalQuantity")?.value;
      const genreId = this.addbookForm.get("genreId")?.value;

      formData.append('Isbn', isbn);
      formData.append('Title', title);
      formData.append('Author', author);
      formData.append('Publication', publication);
      formData.append('PublishedDate', publishedDate);
      formData.append('Edition', edition);
      formData.append('Language', language);
      formData.append('Description', description);
      formData.append('Cost', cost);
      formData.append('AvailableQuantity', availableQuantity);
      formData.append('TotalQuantity', totalQuantity);
      formData.append('GenreId', genreId);

      console.log("Got isbn as: ", isbn);
      console.log("Got title as: ", title);
      console.log("Got author as: ", author);
      console.log("Got publication as: ", publication);
      console.log("Got publishedDate as: ", publishedDate);
      console.log("Got edition as: ", edition);
      console.log("Got language as: ", language);
      console.log("Got description as: ", description);
      console.log("Got cost as: ", cost);
      console.log("Got availableQuantity as: ", availableQuantity);
      console.log("Got totalQuantity as: ", totalQuantity);
      console.log("Got genreId as: ", genreId);

      this.httpClient.post(apiUrl, formData).subscribe((result:any)=>{
        alert("Successfully added Book!");
      },
      (error:any)=>{
        alert("Unsuccessful to add new book!");
      });
      this.displayAddBookForm = false;

    } else {
      alert('Invalid input for one or more fields');
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








