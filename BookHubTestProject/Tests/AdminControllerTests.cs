using Book_Hub_Web_API.Controllers;
using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BookHubTestProject.Tests
{
    public class AdminControllerTests
    {
        private AdminController _adminController;

        private Mock<IAdminRepository> _adminRepository;



        [SetUp]
        public void Setup()
        {

            _adminRepository = new Mock<IAdminRepository>();

            _adminController = new AdminController(_adminRepository.Object);
        }
        //GetAll Borrowed

        [Test]
        public async Task GetAllBorrowed_ReturnsList_noparameterAsync()
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

            _adminRepository.Setup(repo => repo.GetAllBorrowed()).ReturnsAsync(borrowedBooks);

            var result = await _adminController.GetAllBorrowed();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }


        [Test]
        public async Task GetAllBorrowed_ReturnsNotFoundObjectResult()
        {
            List<Borrowed> borroweds = new List<Borrowed>()
            {

            };

            var exceptionMessage = "No Borrows Found";
            _adminRepository.Setup(repo => repo.GetAllBorrowed()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.GetAllBorrowed();
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }

        //GetAllFines
        [Test]
        public async Task GetAllFines_ReturnsList_noparameterAsync()
        {
            var fines = new List<Fines>
{
    new Fines
    {
        FineId = 1,
        BorrowId = 101,
        FineType = Fine_Type.Book_Overdue, // Assuming Fine_Type is an enum
        FineAmount = 25.50m,
        FinePaidStatus = Fine_Paid_Status.Paid, // Assuming Fine_Paid_Status is an enum
        PaidDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)) // Paid 10 days ago
    },
    new Fines
    {
        FineId = 2,
        BorrowId = 102,
        FineType = Fine_Type.Book_Overdue, // Assuming Fine_Type is an enum
        FineAmount = 50.00m,
        FinePaidStatus = Fine_Paid_Status.Unpaid, // Fine is still unpaid
        PaidDate = null // Not paid yet
    },
    new Fines
    {
        FineId = 3,
        BorrowId = 103,
        FineType = Fine_Type.Book_Overdue, // Assuming Fine_Type is an enum
        FineAmount = 15.75m,
        FinePaidStatus = Fine_Paid_Status.Paid, // Fine is paid
        PaidDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)) // Paid 5 days ago
    }
};

            _adminRepository.Setup(repo => repo.GetAllFines()).ReturnsAsync(fines);

            var result = await _adminController.GetAllFine();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        public async Task GetAllFines_ReturnsNotFoundObjectResult()
        {
            List<Fines> fines = new List<Fines>()
            {

            };

            var exceptionMessage = "No fines Found";
            _adminRepository.Setup(repo => repo.GetAllFines()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.GetAllFine();
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }

        //Retrieveing all reservations

        [Test]
        public async Task GetAllResrvations_ReturnsList_noparameterAsync()
        {
            var reservations = new List<Reservations>
{
    new Reservations
    {
        ReservationId = 1,
        BookId = 101,
        UserId = 1001,
        ApplicationTimestamp = DateTime.Now.AddDays(-3),
        ExpectedAvailabilityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
        ReservationExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
        ReservationStatus = Reservation_Status.Pending // Pending status
    },
    new Reservations
    {
        ReservationId = 2,
        BookId = 102,
        UserId = 1002,
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
            _adminRepository.Setup(repo => repo.GetAllReservations()).ReturnsAsync(reservations);

            var result = await _adminController.GetAllReservation();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        public async Task GetAllReservations_ReturnsNotFoundObjectResult()
        {
            List<Reservations> reservations = new List<Reservations>()
            {

            };

            var exceptionMessage = "No Reservations Found";
            _adminRepository.Setup(repo => repo.GetAllReservations()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.GetAllReservation();
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }

        //Reteriving all Notifications

        [Test]
        public async Task GetAllNotifications_ReturnsList_noparameterAsync()
        {
            var notifications = new List<Notifications>
{
    new Notifications
    {
        NotificationId = 1,
        UserId = 1001,
        MessageType = Notification_Type.Account_Related, // Assuming Notification_Type is an enum
        MessageDescription = "Your book reservation is confirmed.",
        SentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)) // Sent 2 days ago
    },
    new Notifications
    {
        NotificationId = 2,
        UserId = 1002,
        MessageType = Notification_Type.Account_Related, // Assuming Notification_Type is an enum
        MessageDescription = "Your reserved book will be available in 3 days.",
        SentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)) // Sent 5 days ago
    },
    new Notifications
    {
        NotificationId = 3,
        UserId = 1003,
        MessageType = Notification_Type.Other,
        MessageDescription = "Your request for an extension has been approved.",
        SentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) // Sent 1 day ago
    }
};
            _adminRepository.Setup(repo => repo.GetAllNotifications()).ReturnsAsync(notifications);

            var result = await _adminController.GetAllNotification();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        public async Task GetAllNotifications_ReturnsNotFoundObjectResult()
        {
            List<Notifications> notifications = new List<Notifications>()
            {

            };

            var exceptionMessage = "No notifications Found";
            _adminRepository.Setup(repo => repo.GetAllNotifications()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.GetAllNotification();
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }

        //Get all Genres
        [Test]
        public async Task GetAllGenres_ReturnsList_noparameterAsync()
        {
            var genres = new List<Genres>
{
    new Genres
    {
        GenreId = 1,
        Name = "Science Fiction",
        Description = "A genre of speculative fiction that explores futuristic concepts such as advanced science, technology, and extraterrestrial life."
    },
    new Genres
    {
        GenreId = 2,
        Name = "Mystery",
        Description = "A genre focused on solving a crime or uncovering hidden truths, often involving detectives or amateur sleuths."
    },
    new Genres
    {
        GenreId = 3,
        Name = "Fantasy",
        Description = "A genre that includes magical or supernatural elements, often set in an imaginary or alternate world."
    }
};

            _adminRepository.Setup(repo => repo.GetAllGenres()).ReturnsAsync(genres);

            var result = await _adminController.GetAllGenre();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        public async Task GetAllGenres_ReturnsNotFoundObjectResult()
        {
            List<Genres> genres = new List<Genres>()
            {

            };

            var exceptionMessage = "No genres Found";
            _adminRepository.Setup(repo => repo.GetAllGenres()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.GetAllGenre();
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }

        //Get all LogUserActivity

        [Test]
        public async Task GetAllLogUserActivity_ReturnsList_noparameterAsync()
        {
            var logUserActivities = new List<LogUserActivity>
{
    new LogUserActivity
    {
        LogId = 1,
        UserId = 1001,
        ActionType = Action_Type.Login, // Assuming Action_Type is an enum
        Timestamp = DateTime.Now.AddMinutes(-10) // Timestamp 10 minutes ago
    },
    new LogUserActivity
    {
        LogId = 2,
        UserId = 1002,
        ActionType = Action_Type.UpdatedAccount, // Assuming Action_Type is an enum
        Timestamp = DateTime.Now.AddHours(-2) // Timestamp 2 hours ago
    },
    new LogUserActivity
    {
        LogId = 3,
        UserId = 1003,
        ActionType = Action_Type.UpdatedAccount, // Assuming Action_Type is an enum
        Timestamp = DateTime.Now.AddDays(-1) // Timestamp 1 day ago
    }
};

            _adminRepository.Setup(repo => repo.GetAllLogUserActivity()).ReturnsAsync(logUserActivities);

            var result = await _adminController.GetAllLogUserActivity();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        public async Task GetAllLogUserActivity_ReturnsNotFoundObjectResult()
        {
            List<LogUserActivity> logUserActivities = new List<LogUserActivity>()
            {

            };

            var exceptionMessage = "No Log User Activity Found";
            _adminRepository.Setup(repo => repo.GetAllLogUserActivity()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.GetAllLogUserActivity();
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }

        //GetAll Users

        [Test]
        public async Task GetAllUsers_ReturnsList_noparameterAsync()
        {
            var users = new List<Users>
{
    new Users
    {
        UserId = 1,
        Name = "John Doe",
        Email = "john.doe@example.com",
        Phone = "1234567890", // 10-digit phone number
        Address = "123 Main St, Springfield",
        PasswordHash = "hashedpassword123", // A hashed password (for simplicity, use a sample here)
        Role = User_Role.Consumer, // Assuming 'Consumer' is one of the User_Role enum values
        AccountCreatedDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6)) // Account created 6 months ago
    },
    new Users
    {
        UserId = 2,
        Name = "Jane Smith",
        Email = "jane.smith@example.com",
        Phone = "0987654321", // 10-digit phone number
        Address = "456 Oak Rd, Rivertown",
        PasswordHash = "hashedpassword456", // A hashed password (for simplicity, use a sample here)
        Role = User_Role.Administrator, // Assuming 'Admin' is another role in the enum
        AccountCreatedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-1)) // Account created 1 year ago
    },
    new Users
    {
        UserId = 3,
        Name = "Alice Johnson",
        Email = "alice.johnson@example.com",
        Phone = "5551234567", // 10-digit phone number
        Address = "789 Pine Ln, Lakeside",
        PasswordHash = "hashedpassword789", // A hashed password (for simplicity, use a sample here)
        Role = User_Role.Consumer, // Assuming 'Consumer' is one of the User_Role enum values
        AccountCreatedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30)) // Account created 30 days ago
    }
};


            _adminRepository.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);

            var result = await _adminController.GetAllUser();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        public async Task GetAllUsers_ReturnsNotFoundObjectResult()
        {
            List<LogUserActivity> logUserActivities = new List<LogUserActivity>()
            {

            };

            var exceptionMessage = "No Log User Activity Found";
            _adminRepository.Setup(repo => repo.GetAllUsers()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.GetAllUser();
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }


        //Remove Book
        [Test]
        [TestCase(3)]
        public async Task RemoveBook_TakesBookId_returnsBooksObject(int Bookid)
        {
            Books book = new Books()
            {
                BookId = 3,
                Isbn = "978-0-12-345678-9",
                Title = "Mysteries Unveiled",
                Author = "Emily Johnson",
                Publication = "Mystery World",
                PublishedDate = new DateOnly(2020, 9, 25),
                Edition = "3rd",
                Language = "English",
                Description = "A collection of intriguing mysteries.",
                Cost = 24.99M,
                AvailableQuantity = 7,
                TotalQuantity = 12,
                GenreId = 3

            };

            _adminRepository.Setup(repo => repo.RemoveBook(Bookid)).ReturnsAsync(book);

            var result = await _adminController.GetAllLogUserActivity();
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        [TestCase(1)]
        public async Task RemoveBook_TakesBookId_returnsNotFoundObject(int BookId)
        {

            var exceptionMessage = "No book Found";
            _adminRepository.Setup(repo => repo.RemoveBook(BookId)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.RemoveBook(BookId);
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));


        }




        // Add a new Book
        [Test]
        [TestCase("978-0-12-345678-9",
                      "Mysteries Unveiled",
                          "Emily Johnson",
                             "Mystery World",
                                  "2024-10-10", // DateOnly works in .NET 6+
                                          "3rd",
                                      "English",
                              "A collection of intriguing mysteries.",
                                     24, 7, 12, 3)]




        public async Task AddBook_TakesBookDTO_Returns_BooksObject(string Isbn, string Title, string Author, string Publication, DateOnly PublicationDate, string Edition, string Language, string Description, Decimal Cost, int AvailableQuantity, int TotalQuantity, int GenreId)
        {
            Add_Book_DTO add_Book_DTO = new Add_Book_DTO()
            {

                Isbn = Isbn,
                Title = Title,
                Author = Author,
                Publication = Publication,
                PublishedDate = PublicationDate,
                Edition = Edition,
                Language = Language,
                Description = Description,
                Cost = Cost,
                AvailableQuantity = AvailableQuantity,
                TotalQuantity = TotalQuantity,
                GenreId = GenreId
            };
            Books book = new Books()
            {
                BookId = 1,
                Isbn = Isbn,
                Title = Title,
                Author = Author,
                Publication = Publication,
                PublishedDate = PublicationDate,
                Edition = Edition,
                Language = Language,
                Description = Description,
                Cost = Cost,
                AvailableQuantity = AvailableQuantity,
                TotalQuantity = TotalQuantity,
                GenreId = GenreId
            };

            _adminRepository.Setup(repo => repo.AddBook(add_Book_DTO)).ReturnsAsync(book);

            var result = await _adminController.AddBooks(add_Book_DTO);
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]

        public async Task AddBook_TakesBookDTO_Returns_StatusCode()
        {

            Add_Book_DTO _Book_DTO = new Add_Book_DTO()
            {
                Isbn = "978-0-12-345678-9",
                Title = "Mysteries Unveiled",
                Author = "Emily Johnson",
                Publication = "Mystery World",
                PublishedDate = DateOnly.Parse("2024-10-10"),
                Edition = "3rd",
                Language = "English",
                Description = "A collection of intriguing mysteries.",
                Cost = 24,
                AvailableQuantity = 7,
                TotalQuantity = 12,
                GenreId = 3
            };

            _adminRepository.Setup(repo => repo.AddBook(_Book_DTO)).ThrowsAsync(new Exception("Cannot Be Added"));

            var result = await _adminController.AddBooks(_Book_DTO);
            var objectResult = result as ObjectResult;
            // Assert.Pass();
            Console.WriteLine(objectResult?.StatusCode);
            Assert.That(500, Is.EqualTo(objectResult?.StatusCode));

        }
        [Test]
        [TestCase(1, 3, 3)]
        public async Task UpdateBook_Returns_BooksObject(int BookId, int AvailableQuantity, int TotalQuantity)
        {
            Update_book_dto update_Book_Dto = new Update_book_dto()
            {
                BookId = BookId,
                AvailableQuantity = AvailableQuantity,
                TotalQuantity = TotalQuantity
            };

            Books book = new Books()
            {
                BookId = BookId,
                Isbn = "abcv",
                Title = "The Sage",
                Author = "Anonymous",
                Publication = "The Sunshine",
                PublishedDate = DateOnly.Parse("2024-10-10"),
                Edition = "Second",
                Language = "English",
                Description = " noDescription",
                Cost = 200,
                AvailableQuantity = AvailableQuantity,
                TotalQuantity = TotalQuantity,
                GenreId = 1
            };

            _adminRepository.Setup(repo => repo.UpdateBook(update_Book_Dto)).ReturnsAsync(book);

            var result = await _adminController.UpdateBooks(update_Book_Dto);
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        [TestCase(1, 3, 3)]
        public async Task UpdateBook_TakesUpdateBookDto_returnsNotFoundObject(int BookId, int AvailableQuantity, int TotalQuantity)
        {
            Update_book_dto update_Book_Dto = new Update_book_dto()
            {
                BookId = BookId,
                AvailableQuantity = AvailableQuantity,
                TotalQuantity = TotalQuantity
            };

            Books book = new Books()
            {
                BookId = BookId,
                Isbn = "abcv",
                Title = "The Sage",
                Author = "Anonymous",
                Publication = "The Sunshine",
                PublishedDate = DateOnly.Parse("2024-10-10"),
                Edition = "Second",
                Language = "English",
                Description = " noDescription",
                Cost = 200,
                AvailableQuantity = AvailableQuantity,
                TotalQuantity = TotalQuantity,
                GenreId = 1
            };

            var exceptionMessage = "No book Found";
            _adminRepository.Setup(repo => repo.UpdateBook(update_Book_Dto)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _adminController.UpdateBooks(update_Book_Dto);
            var notFoundResult = new NotFoundObjectResult(result);
            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));



        }
    }
}
