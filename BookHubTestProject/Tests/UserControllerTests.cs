using System.Collections.Generic;
using System.Net;
using Book_Hub_Web_API.Controllers;
using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace BookHubTestProject.Tests
{
    public class UserControllerTests
    {
        private UserController _userController;

        private Mock<IUserRepository> _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();

            _userController = new UserController(_userRepository.Object);
        }

        [Test]
        [TestCase(3)]
        [TestCase(22)]
        [TestCase(486)]
        public async Task GetBookByBookId_BookId_ReturnsOkObjectResult(int bookId)
        {
            Books book = new Books()
            {
                BookId = bookId,
                Isbn = "978-0-12-345678-9",
                Title = "Romance in Paris",
                Author = "Emily Brown",
                Publication = "Romantic Reads",
                PublishedDate = DateOnly.Parse("2021-05-10"),
                Edition = "1st Edition",
                Language = "English",
                Description = "A heartwarming romance set against the backdrop of Paris.",
                Cost = 9.99M,
                AvailableQuantity = 100,
                TotalQuantity = 100,
                GenreId = 8
            };
            _userRepository.Setup(repo => repo.GetBookByBookId(bookId)).ReturnsAsync(book);

            var result = await _userController.GetBookByBookId(bookId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(3)]
        [TestCase(22)]
        [TestCase(486)]
        public async Task GetBookByBookId_BookId_ReturnsBadObjectResult(int bookId)
        {
            var exceptionMessage = $"No such book found! Book Id: {bookId}";
            _userRepository.Setup(repo => repo.GetBookByBookId(bookId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.GetBookByBookId(bookId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase("978-1-23-456789-0")]
        [TestCase("978-1-55-555555-0")]
        [TestCase("978-0-34-876543-4")]
        public async Task GetBookByISBN_ISBN_ReturnsOkObjectResult(string isbn)
        {
            Books book = new Books()
            {
                BookId = 1,
                Isbn = isbn,
                Title = "Romance in Paris",
                Author = "Emily Brown",
                Publication = "Romantic Reads",
                PublishedDate = DateOnly.Parse("2021-05-10"),
                Edition = "1st Edition",
                Language = "English",
                Description = "A heartwarming romance set against the backdrop of Paris.",
                Cost = 9.99M,
                AvailableQuantity = 100,
                TotalQuantity = 100,
                GenreId = 8
            };
            _userRepository.Setup(repo => repo.GetBookByISBN(isbn)).ReturnsAsync(book);

            var result = await _userController.GetBookByISBN(isbn);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase("978-1-23-456789-0")]
        [TestCase("978-1-55-555555-0")]
        [TestCase("978-0-34-876543-4")]
        public async Task GetBookByISBN_ISBN_ReturnsBadObjectResult(string isbn)
        {
            var exceptionMessage = $"No such book found! Book ISBN No.: {isbn}";
            _userRepository.Setup(repo => repo.GetBookByISBN(isbn)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.GetBookByISBN(isbn);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task GetBooksByGenre_GenreId_ReturnsOkObjectResult(int genreId)
        {
            List<Books> bookList = new List<Books>()
            {
                new Books() { BookId = 2,  Isbn = "978-0-12-345678-9", Title = "Romance in Paris", Author = "Emily Brown", Publication = "Romantic Reads", PublishedDate = DateOnly.Parse("2021-05-10"), Edition = "1st Edition", Language = "English", Description = "A heartwarming romance set against the backdrop of Paris.", Cost = 9.99M, AvailableQuantity = 100, TotalQuantity = 100, GenreId = genreId },
                new Books() { BookId = 5,  Isbn = "978-0-12-345678-9", Title = "Romance in Paris", Author = "Emily Brown", Publication = "Romantic Reads", PublishedDate = DateOnly.Parse("2021-05-10"), Edition = "1st Edition", Language = "English", Description = "A heartwarming romance set against the backdrop of Paris.", Cost = 9.99M, AvailableQuantity = 100, TotalQuantity = 100, GenreId = genreId },
                new Books() { BookId = 52, Isbn = "978-0-12-345678-9", Title = "Romance in Paris", Author = "Emily Brown", Publication = "Romantic Reads", PublishedDate = DateOnly.Parse("2021-05-10"), Edition = "1st Edition", Language = "English", Description = "A heartwarming romance set against the backdrop of Paris.", Cost = 9.99M, AvailableQuantity = 100, TotalQuantity = 100, GenreId = genreId }
            };
            _userRepository.Setup(repo => repo.GetBooksByGenre(genreId)).ReturnsAsync(bookList);

            var result = await _userController.GetBooksByGenre(genreId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task GetBooksByGenre_GenreId_ReturnsBadObjectResult(int genreId)
        {
            var exceptionMessage = $"No book found for Genre Id: {genreId}";
            _userRepository.Setup(repo => repo.GetBooksByGenre(genreId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.GetBooksByGenre(genreId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase("Emily Brown")]
        [TestCase("Rebecca White")]
        [TestCase("Anna Roberts")]
        public async Task GetBooksByAuthor_Author_ReturnsOkObjectResult(string authorName)
        {
            List<Books> bookList = new List<Books>()
            {
                new Books() { BookId = 2,  Isbn = "978-0-12-345678-9", Title = "Romance in Paris", Author = authorName, Publication = "Romantic Reads", PublishedDate = DateOnly.Parse("2021-05-10"), Edition = "1st Edition", Language = "English", Description = "A heartwarming romance set against the backdrop of Paris.", Cost = 9.99M, AvailableQuantity = 100, TotalQuantity = 100, GenreId = 8 },
                new Books() { BookId = 5,  Isbn = "978-0-12-345678-9", Title = "Romance in Paris", Author = authorName, Publication = "Romantic Reads", PublishedDate = DateOnly.Parse("2021-05-10"), Edition = "1st Edition", Language = "English", Description = "A heartwarming romance set against the backdrop of Paris.", Cost = 9.99M, AvailableQuantity = 100, TotalQuantity = 100, GenreId = 8 },
                new Books() { BookId = 52, Isbn = "978-0-12-345678-9", Title = "Romance in Paris", Author = authorName, Publication = "Romantic Reads", PublishedDate = DateOnly.Parse("2021-05-10"), Edition = "1st Edition", Language = "English", Description = "A heartwarming romance set against the backdrop of Paris.", Cost = 9.99M, AvailableQuantity = 100, TotalQuantity = 100, GenreId = 8 }
            };
            _userRepository.Setup(repo => repo.GetBooksByAuthor(authorName)).ReturnsAsync(bookList);

            var result = await _userController.GetBooksByAuthor(authorName);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase("Emily Brown")]
        [TestCase("Rebecca White")]
        [TestCase("Anna Roberts")]
        public async Task GetBooksByAuthor_Author_ReturnsBadObjectResult(string authorName)
        {
            var exceptionMessage = $"No book found for Author: {authorName}";
            _userRepository.Setup(repo => repo.GetBooksByAuthor(authorName)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.GetBooksByAuthor(authorName);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase(3, 1)]
        [TestCase(8, 46)]
        [TestCase(1, 3)]
        public async Task BorrowBook_BookIdAndUserId_ReturnsOkObjectResult(int bookId, int userId)
        {
            Borrowed borrowed = new Borrowed()
            {
                BorrowId = 1,
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                ReturnDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                ReturnDate = null,
                BorrowStatus = Borrow_Status.Borrowed
            };
            _userRepository.Setup(repo => repo.BorrowBook(bookId, userId)).ReturnsAsync(borrowed);

            var result = await _userController.BorrowBook(bookId, userId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(3, 1, $"Incorrect Book Id: 3!")]
        [TestCase(3, 1, $"Insufficient Book quantity for Book Id: 3!")]
        [TestCase(3, 1, $"Book with Id: 3 is already borrowed by someone else!")]
        [TestCase(3, 1, $"Book with Id: 3 is already borrowed by you!")]
        [TestCase(3, 1, $"Book with Id: 3 is unavailable because of existing reservations!")]
        [TestCase(3, 1, $"Borrow Request Declined. Your Reservation Id: 1 is expired, and reservations by other users exist!")]
        [TestCase(3, 1, $"You already have borrowed 5 books! Only 5 books max are allowed to borrow. Return some and try again.")]
        public async Task BorrowBook_BookIdAndUserId_ReturnsBadObjectResult(int bookId, int userId, string exceptionMessage)
        {
            Reservations alreadyReserved = new Reservations()
            {
                ReservationId = 1
            };
            List<Borrowed> totalBorrows = new List<Borrowed>()
            {
                new Borrowed(){},
                new Borrowed(){},
                new Borrowed(){},
                new Borrowed(){},
                new Borrowed(){},
            };

            List<string> exceptionMessagesList = new List<string>()
            {
                $"Incorrect Book Id: {bookId}!",
                $"Insufficient Book quantity for Book Id: {bookId}!",
                $"Book with Id: {bookId} is already borrowed by someone else!",
                $"Book with Id: {bookId} is already borrowed by you!",
                $"Book with Id: {bookId} is unavailable because of existing reservations!",
                $"Borrow Request Declined. Your Reservation Id: {alreadyReserved.ReservationId} is expired, and reservations by other users exist!",
                $"You already have borrowed {totalBorrows.Count} books! Only {5} books max are allowed to borrow. Return some and try again."
            };

            _userRepository.Setup(repo => repo.BorrowBook(bookId, userId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.BorrowBook(bookId, userId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessagesList.Contains(exceptionMessage), Is.True);
        }

        [Test]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(9)]
        public async Task ReturnBook_BorrowId_ReturnsOkObjectResult(int borrowId)
        {
            Borrowed borrowed = new Borrowed()
            {
                BorrowId = borrowId,
                BookId = 1,
                UserId = 1,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                ReturnDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                ReturnDate = DateOnly.FromDateTime(DateTime.Now),
                BorrowStatus = Borrow_Status.Borrowed
            };
            _userRepository.Setup(repo => repo.ReturnBook(borrowId)).ReturnsAsync(borrowed);

            var result = await _userController.ReturnBook(borrowId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(45)]
        [TestCase(626)]
        [TestCase(324)]
        public async Task ReturnBook_BorrowId_ReturnsBadObjectResult(int borrowId)
        {
            var exceptionMessage = $"Borrow Id: {borrowId} does not exist!";
            _userRepository.Setup(repo => repo.ReturnBook(borrowId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.ReturnBook(borrowId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(9)]
        public async Task ReportLostBook_BorrowId_ReturnsOkObjectResult(int borrowId)
        {
            Borrowed borrowed = new Borrowed()
            {
                BorrowId = borrowId,
                BookId = 1,
                UserId = 1,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                ReturnDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                ReturnDate = null,
                BorrowStatus = Borrow_Status.Lost
            };
            _userRepository.Setup(repo => repo.ReturnBook(borrowId)).ReturnsAsync(borrowed);

            var result = await _userController.ReturnBook(borrowId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(45)]
        [TestCase(626)]
        [TestCase(324)]
        public async Task ReportLostBook_BorrowId_ReturnsBadObjectResult(int borrowId)
        {
            var exceptionMessage = $"Borrow Id: {borrowId} does not exist!";
            _userRepository.Setup(repo => repo.ReturnBook(borrowId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.ReturnBook(borrowId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase(3, 1)]
        [TestCase(8, 46)]
        [TestCase(1, 3)]
        public async Task ReserveBook_BookIdAndUserId_ReturnsOkObjectResult(int bookId, int userId)
        {
            Reservations reservation = new Reservations()
            {
                BookId = bookId,
                UserId = userId,
                ApplicationTimestamp = DateTime.Now,
                ExpectedAvailabilityDate = null,
                ReservationExpiryDate = null,
                ReservationStatus = Reservation_Status.Pending
            };
            _userRepository.Setup(repo => repo.ReserveBook(bookId, userId)).ReturnsAsync(reservation);

            var result = await _userController.BorrowBook(bookId, userId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(3, 1, "You cannot reserve a book you have already borrowed! BorrowId: 1")]
        [TestCase(3, 6, "You already have a reservation for this book! Reservation Id: 3")]
        public async Task ReserveBook_BookIdAndUserId_ReturnsBadObjectResult(int bookId, int userId, string exceptionMessage)
        {
            Borrowed? alreadyBorrowed = new Borrowed()
            {
                BorrowId = 1,
                BookId = bookId,
                BorrowStatus = Borrow_Status.Borrowed
            };

            List<Reservations> reservationsList = new List<Reservations>()
            {
                new Reservations(){ ReservationId = 1,  BookId = bookId, UserId = 8, ReservationStatus = Reservation_Status.Pending, ApplicationTimestamp = DateTime.Now.AddDays(-3) },
                new Reservations(){ ReservationId = 2,  BookId = bookId, UserId = 3, ReservationStatus = Reservation_Status.Pending, ApplicationTimestamp = DateTime.Now.AddDays(-2) },
                new Reservations(){ ReservationId = 3,  BookId = bookId, UserId = 6, ReservationStatus = Reservation_Status.Pending, ApplicationTimestamp = DateTime.Now.AddDays(-1) }
            };

            Reservations? existingUserReservation = reservationsList.Where(r => r.UserId == userId).FirstOrDefault();

            List<string> exceptionMessagesList = new List<string>()
            {
                $"You cannot reserve a book you have already borrowed! BorrowId: {alreadyBorrowed.BorrowId}",
                $"You already have a reservation for this book! Reservation Id: {existingUserReservation?.ReservationId}"
            };

            _userRepository.Setup(repo => repo.ReserveBook(bookId, userId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.ReserveBook(bookId, userId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            foreach (string expMessage in exceptionMessagesList)
            {
                Console.Write(expMessage + ", ");
            }
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessagesList.Contains(exceptionMessage), Is.True);
        }

        [Test]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(9)]
        public async Task CancelBookReservation_ReservationId_ReturnsOkObjectResult(int reservationId)
        {
            Reservations reservation = new Reservations()
            {
                ReservationId = reservationId,
                BookId = 2,
                UserId = 5,
                ApplicationTimestamp = DateTime.Now,
                ExpectedAvailabilityDate = null,
                ReservationExpiryDate = null,
                ReservationStatus = Reservation_Status.Pending
            };
            _userRepository.Setup(repo => repo.CancelBookReservation(reservationId)).ReturnsAsync(reservation);

            var result = await _userController.CancelBookReservation(reservationId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(45)]
        [TestCase(626)]
        [TestCase(324)]
        public async Task CancelBookReservation_ReservationId_ReturnsBadObjectResult(int reservationId)
        {
            var exceptionMessage = $"Reservation Id: {reservationId} does not exist";
            _userRepository.Setup(repo => repo.CancelBookReservation(reservationId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.CancelBookReservation(reservationId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase(2, "tony@gmail.com", "tony@avengers1", "tony@avengers123")]
        [TestCase(5, "steve@gmail.com", "steve@avengers2", "steve@avengers123")]
        [TestCase(9, "sam@gmail.com", "sam@avengers3", "tony@avengers123")]
        public async Task ResetPassword_Reset_Password_DTO_ReturnsOkObjectResult(int userId, string email, string oldPassword, string newPassword)
        {
            Reset_Password_DTO reset_Password_DTO = new Reset_Password_DTO()
            {
                UserId = userId,
                Email = email,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            Users user = new Users()
            {
                UserId = userId,
                Email = email,
                PasswordHash = newPassword
            };

            _userRepository.Setup(repo => repo.ResetPassword(reset_Password_DTO)).ReturnsAsync(user);

            var result = await _userController.ResetPassword(reset_Password_DTO);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase(2, "nonexistentindbtony@gmail.com", "tony@avengers1", "tony@avengers123", $"User Id: 2 not found!")]
        [TestCase(5, "steve@gmail.com", "randominvalidpassword", "steve@avengers123", $"Old password does not match")]
        public async Task ResetPassword_Reset_Password_DTO_ReturnsBadObjectResult(int userId, string email, string oldPassword, string newPassword, string exceptionMessage)
        {
            Reset_Password_DTO reset_Password_DTO = new Reset_Password_DTO()
            {
                UserId = userId,
                Email = email,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            List<string> exceptionMessagesList = new List<string>()
            {
                $"User Id: {reset_Password_DTO.UserId} not found!",
                $"Old password does not match"
            };

            _userRepository.Setup(repo => repo.ResetPassword(reset_Password_DTO)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.ResetPassword(reset_Password_DTO);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        //New Test Methods 

        [Test]
        [TestCase(1)]
        public async Task GetNotificationsByUserId_Returns_ListOfNotifications(int userId)
        {
            var notifications = new List<Notifications>
{
    new Notifications
    {
        NotificationId = 1,
        UserId = 1,
        MessageType = Notification_Type.Account_Related, // Assuming Notification_Type is an enum
        MessageDescription = "Your book reservation is confirmed.",
        SentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)) // Sent 2 days ago
    },
    new Notifications
    {
        NotificationId = 2,
        UserId = 1,
        MessageType = Notification_Type.Account_Related, // Assuming Notification_Type is an enum
        MessageDescription = "Your reserved book will be available in 3 days.",
        SentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)) // Sent 5 days ago
    },
    new Notifications
    {
        NotificationId = 3,
        UserId = 1,
        MessageType = Notification_Type.Other,
        MessageDescription = "Your request for an extension has been approved.",
        SentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) // Sent 1 day ago
    }
};
          
            _userRepository.Setup(repo => repo.GetNotificationsByUserId(userId)).ReturnsAsync(notifications);

            var result = await _userController.GetNotificationsByUserId(userId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        //If the list is emp

        [Test]
        [TestCase(1)]
        public async Task GetNotificationsByUserId_Returns_NotFoundObject(int userId)
        {
          
            var exceptionMessage = "No notifications found!";


            _userRepository.Setup(repo => repo.GetNotificationsByUserId(userId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.GetNotificationsByUserId(userId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));

        }


        [Test]
        [TestCase(3)]
        public async Task GetReservationsByUserId_Returns_ListOfNotifications(int userId)
        {
            var reservations = new List<Reservations>
{
    new Reservations
    {
        ReservationId = 1,
        BookId = 101,
        UserId = 3,
        ApplicationTimestamp = DateTime.Now.AddDays(-3),
        ExpectedAvailabilityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
        ReservationExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
        ReservationStatus = Reservation_Status.Pending // Pending status
    },
    new Reservations
    {
        ReservationId = 2,
        BookId = 102,
        UserId = 3,
        ApplicationTimestamp = DateTime.Now.AddDays(-10), // Application timestamp 10 days ago
        ExpectedAvailabilityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), // Expected availability in 5 days
        ReservationExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(12)), // Reservation expiry in 12 days
        ReservationStatus = Reservation_Status.Fullfilled // Confirmed status
    },
    new Reservations
    {
        ReservationId = 3,
        BookId = 103,
        UserId = 1003,
        ApplicationTimestamp = DateTime.Now.AddDays(-1), // Application timestamp 1 day ago
        ExpectedAvailabilityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), // Expected availability in 3 days
        ReservationExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)), // Reservation expiry in 10 days
        ReservationStatus = Reservation_Status.Pending // Pending status
    }
};

            _userRepository.Setup(repo => repo.GetReservationsByUserId(userId)).ReturnsAsync(reservations);

            var result = await _userController.GetReservationsByUserId(userId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }
        //if list is empty
        [Test]
        [TestCase(1)]
        public async Task GetReservationsByUserId_Returns_BadRequestObject(int userId)
        {
           
            var exceptionMessage = "No reservations found!";


            _userRepository.Setup(repo => repo.GetReservationsByUserId(userId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.GetReservationsByUserId(userId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));

        }



        [Test]
        [TestCase(1)]
        public async Task GetBorrowedByUserId_Returns_ListOfNotifications(int userId)
        {
            List<Borrowed> borrowedBooks = new List<Borrowed>
        {
            new Borrowed
            {
                BorrowId = 1,
                BookId = 101,
                UserId = 1001,
                BorrowDate = new DateOnly(2024, 11, 1),
                ReturnDeadline = new DateOnly(2024, 12, 1),
                ReturnDate = null, // Not yet returned
                BorrowStatus = Borrow_Status.Borrowed
            },
            new Borrowed
            {
                BorrowId = 2,
                BookId = 102,
                UserId = 1002,
                BorrowDate = new DateOnly(2024, 11, 5),
                ReturnDeadline = new DateOnly(2024, 12, 5),
                ReturnDate = null, // Not yet returned
                BorrowStatus = Borrow_Status.Borrowed
            },
            new Borrowed
            {
                BorrowId = 3,
                BookId = 103,
                UserId = 1003,
                BorrowDate = new DateOnly(2024, 11, 10),
                ReturnDeadline = new DateOnly(2024, 12, 10),
                ReturnDate = new DateOnly(2024, 11, 25), // Returned early
                BorrowStatus = Borrow_Status.Returned
            }
        };

            _userRepository.Setup(repo => repo.GetBorrowedByUserId(userId)).ReturnsAsync(borrowedBooks);

            var result = await _userController.GetBorrowedByUserId(userId);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }
        //if list is empty 




        [Test]
        [TestCase(1)]
        public async Task GetBorrowedByUserId_Returns_BadRequestObject(int userId)
        {

            var exceptionMessage = "No borrowings found!";


            _userRepository.Setup(repo => repo.GetBorrowedByUserId(userId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _userController.GetBorrowedByUserId(userId);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));

        }












    }
}
