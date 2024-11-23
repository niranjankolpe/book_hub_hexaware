using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Data;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Data.DTO;

namespace Book_Hub_Web_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookHubDBContext _context;

        private readonly List<Genres> _genreList = new List<Genres>()
        {
            new Genres () {GenreId=1, Name="Science Fiction", Description="Science and Tech Genre"},
            new Genres () {GenreId=2, Name="Romance", Description="Romance Genre"}
        };

        private readonly List<Books> _bookList = new List<Books>()
        {
            new Books() {BookId = 1, Isbn="234", Title="A trip to future", Author="Niranjan", Cost=342, AvailableQuantity=5, TotalQuantity=5, GenreId=1},
            new Books() {BookId = 2, Isbn="357", Title="Paris Romance", Author="Niranjan", Cost=862, AvailableQuantity=7, TotalQuantity=7, GenreId=2}
        };

        private readonly List<Users> _usersList = new List<Users>()
        {
            new Users() {UserId=1, Name="Niranjan", Email="niranjan@gmail.com", Phone="3462462466", Address="Mumbai", PasswordHash="niranjan@pass", Role=User_Role.Consumer, AccountCreatedDate=DateOnly.FromDateTime(DateTime.Now)}
        };

        private readonly List<Borrowed> _borrowedList = new List<Borrowed>()
        {
            new Borrowed(){BorrowId=1, BookId=1, UserId=1, BorrowDate=DateOnly.FromDateTime(DateTime.Now), ReturnDeadline=DateOnly.FromDateTime(DateTime.Now.AddDays(10)), ReturnDate=null, BorrowStatus=Borrow_Status.Borrowed}
        };

        private readonly List<Reservations> _reservationsList = new List<Reservations>()
        {
            new Reservations() { ReservationId=1, BookId=1, UserId=1, ApplicationTimestamp=DateTime.Now, ExpectedAvailabilityDate=null, ReservationExpiryDate=null, ReservationStatus=Reservation_Status.Pending}
        };

        public UserRepository(BookHubDBContext context)
        {
            _context = context;
        }
        public async Task<Books> GetBookByBookId(int bookId)
        {
            await Task.Delay(100);
            // Books book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            Books book = _bookList.FirstOrDefault(b => b.BookId == bookId);
            return book;
        }

        public async Task<Books> GetBookByISBN(string isbn)
        {
            await Task.Delay(100);
            Books book = _bookList.FirstOrDefault(b => b.Isbn == isbn);
            return book;
        }

        public async Task<List<Books>> GetBooksByGenre(int genreId)
        {
            await Task.Delay(100);
            List<Books> books = _bookList.FindAll(b => b.GenreId == genreId).ToList();
            return books;
        }

        public async Task<List<Books>> GetBooksByAuthor(string authorName)
        {
            await Task.Delay(100);
            List<Books> books = _bookList.FindAll(b => b.Author == authorName).ToList();
            return books;
        }

        public async Task<Borrowed> BorrowBook(int bookId, int userId)
        {
            await Task.Delay(100);
            Borrowed borrowed = new Borrowed()
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                ReturnDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                ReturnDate = null,
                BorrowStatus = Borrow_Status.Borrowed
            };
            _borrowedList.Add(borrowed);
            return borrowed;
        }

        public async Task<Borrowed> ReturnBook(int borrowId)
        {
            await Task.Delay(100);
            Borrowed borrow = _borrowedList.FirstOrDefault(b => b.BorrowId == borrowId);
            _borrowedList.Remove(borrow);
            return borrow;
        }

        public async Task<Borrowed> ReportLostBook(int borrowId)
        {
            await Task.Delay(100);
            Borrowed borrow = _borrowedList.FirstOrDefault(b => b.BorrowId == borrowId);
            borrow.BorrowStatus = Borrow_Status.Lost;
            return borrow;
        }

        public async Task<Reservations> ReserveBook(int bookId, int userId)
        {
            await Task.Delay(100);
            Reservations reservation = new Reservations()
            {
                BookId = bookId,
                UserId = userId,
                ApplicationTimestamp = DateTime.Now,
                ExpectedAvailabilityDate = null,
                ReservationExpiryDate = null,
                ReservationStatus = Reservation_Status.Pending
            };
            _reservationsList.Add(reservation);
            return reservation;
        }

        public async Task<Reservations> CancelBookReservation(int reservationId)
        {
            await Task.Delay(100);
            Reservations reservation = _reservationsList.FirstOrDefault(r => r.ReservationId == reservationId);
            _reservationsList.Remove(reservation);
            return reservation;
        }

        public async Task<Users> ResetPassword(Reset_Password_DTO reset_Password_DTO)
        {
            await Task.Delay(100);
            Users existingUser = _usersList.FirstOrDefault(u => u.UserId == reset_Password_DTO.UserId);

            if (existingUser.PasswordHash == reset_Password_DTO.OldPassword)
            {
                existingUser.PasswordHash = reset_Password_DTO.NewPassword;
            }
            // _context.Update(user);
            // await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}
