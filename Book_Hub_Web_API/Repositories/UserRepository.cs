using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Data;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Data.DTO;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Book_Hub_Web_API.Repositories
{
    public class UserRepository(BookHubDBContext context) : IUserRepository
    {
        private readonly BookHubDBContext _context = context;



        public async Task<Books> GetBookByBookId(int bookId)
        {
            await Task.Delay(100);
            Books book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            return book;
        }



        public async Task<Books> GetBookByISBN(string isbn)
        {
            await Task.Delay(100);
            Books book = _context.Books.FirstOrDefault(b => b.Isbn == isbn);
            return book;
        }



        public async Task<List<Books>> GetBooksByGenre(int genreId)
        {
            await Task.Delay(100);
            List<Books> books = _context.Books.Where(b => b.GenreId == genreId).ToList();
            return books;
        }



        public async Task<List<Books>> GetBooksByAuthor(string authorName)
        {
            await Task.Delay(100);
            List<Books> books = _context.Books.Where(b => b.Author == authorName).ToList();
            return books;
        }



        public async Task<Borrowed> BorrowBook(int bookId, int userId)
        {
            Borrowed borrowed = new Borrowed()
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                ReturnDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                ReturnDate = null,
                BorrowStatus = Borrow_Status.Borrowed
            };
            _context.Add(borrowed);
            await _context.SaveChangesAsync();

            //Fines f1 = new() { BorrowId = borrowed.BorrowId, FineType = Fine_Type.Book_Overdue, FineAmount = 0, FinePaidStatus = Fine_Paid_Status.Unpaid, PaidDate = null };
            //_context.Add(f1);
            //await _context.SaveChangesAsync();

            //Fines f2 = new() { BorrowId = borrowed.BorrowId, FineType = Fine_Type.Book_Damage, FineAmount = 0, FinePaidStatus = Fine_Paid_Status.Unpaid, PaidDate = null };
            //_context.Add(f2);
            //await _context.SaveChangesAsync();

            //Fines f3 = new() { BorrowId = borrowed.BorrowId, FineType = Fine_Type.Book_Lost, FineAmount = 0, FinePaidStatus = Fine_Paid_Status.Unpaid, PaidDate = null };
            //_context.Add(f3);
            //await _context.SaveChangesAsync();

            return borrowed;
        }


        
        public async Task<Borrowed> ReturnBook(int borrowId)
        {
            Borrowed borrowed = _context.Borrowed.FirstOrDefault(b => b.BorrowId == borrowId);
            borrowed.BorrowStatus = Borrow_Status.Returned;
            _context.Update(borrowed);
            await _context.SaveChangesAsync();

            //Fines f1 = _context.Fines.FirstOrDefault(f => f.BorrowId == borrowId);
            //f1.FineAmount = 0;
            //f1.FinePaidStatus = Fine_Paid_Status.Paid;
            //f1.PaidDate = DateOnly.FromDateTime(DateTime.Now);
            //_context.Update(f1);
            //await _context.SaveChangesAsync();

            //Fines f2 = _context.Fines.FirstOrDefault(f => f.BorrowId == borrowId);
            //f2.FineAmount = 0;
            //f2.FinePaidStatus = Fine_Paid_Status.Paid;
            //f2.PaidDate = DateOnly.FromDateTime(DateTime.Now);
            //_context.Update(f2);
            //await _context.SaveChangesAsync();

            //Fines f3 = _context.Fines.FirstOrDefault(f => f.BorrowId == borrowId);
            //f3.FineAmount = 0;
            //f3.FinePaidStatus = Fine_Paid_Status.Paid;
            //f3.PaidDate = DateOnly.FromDateTime(DateTime.Now);
            //_context.Update(f3);
            //await _context.SaveChangesAsync();

            return borrowed;
        }



        public async Task<Borrowed> ReportLostBook(int borrowId)
        {
            Borrowed borrowed = _context.Borrowed.FirstOrDefault(b => b.BorrowId == borrowId);
            borrowed.BorrowStatus = Borrow_Status.Lost;
            _context.Update(borrowed);
            await _context.SaveChangesAsync();

            List<Fines> fines = _context.Fines.Where(f => f.BorrowId == borrowId).ToList();
            foreach (Fines fine in fines)
            {
                fine.FineAmount = 0;
                fine.FinePaidStatus = Fine_Paid_Status.Paid;
                fine.PaidDate = DateOnly.FromDateTime(DateTime.Now);
            }
            _context.UpdateRange(fines);
            await _context.SaveChangesAsync();

            return borrowed;
        }



        public async Task<Reservations> ReserveBook(int bookId, int userId)
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

            Borrowed b = _context.Borrowed.FirstOrDefault(b => b.BookId == bookId && b.BorrowStatus==Borrow_Status.Borrowed);
            Reservations r = _context.Reservations.Where(r => r.BookId == bookId && r.ReservationStatus == Reservation_Status.Pending).OrderBy(r => r.ApplicationTimestamp).FirstOrDefault();

            if (b == null && r==null)
            {
                reservation.ExpectedAvailabilityDate = DateOnly.FromDateTime(DateTime.Now);
                reservation.ReservationExpiryDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
                reservation.ReservationStatus = Reservation_Status.Pending;
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                Notifications notification = new Notifications()
                {
                    UserId = userId,
                    MessageType = Notification_Type.Reservation_Book_Related,
                    MessageDescription = $"Your reserved book with Book Id: {bookId} is available for borrowing.",
                    SentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return reservation;
            }

            if (b != null)
            {
                reservation.ExpectedAvailabilityDate = b.ReturnDeadline.AddDays(1);
                reservation.ReservationExpiryDate = null;
                reservation.ReservationStatus = Reservation_Status.Pending;
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                Notifications notification = new Notifications()
                {
                    UserId = userId,
                    MessageType = Notification_Type.Reservation_Book_Related,
                    MessageDescription = $"Applied for reservation with Book Id: {bookId}",
                    SentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
            else if (r != null)
            {
                if (r.ExpectedAvailabilityDate != null)
                {
                    reservation.ExpectedAvailabilityDate = r.ExpectedAvailabilityDate.Value.AddDays(10);
                }
                else
                {
                    reservation.ExpectedAvailabilityDate = null;
                }
                reservation.ReservationExpiryDate = null;
                reservation.ReservationStatus = Reservation_Status.Pending;
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                Notifications notification = new Notifications()
                {
                    UserId = userId,
                    MessageType = Notification_Type.Reservation_Book_Related,
                    MessageDescription = $"Applied for reservation with Book Id: {bookId}",
                    SentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
            return reservation;
        }



        public async Task<Reservations> CancelBookReservation(int reservationId)
        {
            Reservations reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            reservation.ReservationStatus = Reservation_Status.Cancelled;
            _context.Update(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }



        public async Task<Users> ResetPassword(Reset_Password_DTO reset_Password_DTO)
        {
            //Users existingUser = _usersList.FirstOrDefault(u => u.UserId == reset_Password_DTO.UserId);
            Users existingUser = _context.Users.Where(u => u.UserId == reset_Password_DTO.UserId).FirstOrDefault();

            if (existingUser == null)
            {
                return new Users() { PasswordHash=reset_Password_DTO.NewPassword};
            }

            existingUser.PasswordHash = reset_Password_DTO.NewPassword;
            _context.Update(existingUser);
            await _context.SaveChangesAsync();

            Notifications notification = new Notifications()
            {
                UserId = existingUser.UserId,
                MessageType = Notification_Type.Account_Related,
                MessageDescription = $"Password Successfully Updated!",
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Add(notification);
            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}
