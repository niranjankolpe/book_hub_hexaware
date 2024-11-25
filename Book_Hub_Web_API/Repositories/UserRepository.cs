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
            Books? book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
            return book != null ? book : throw new Exception($"No such book found! Book Id: {bookId}");
            // return book ?? new Books();
        }

        public async Task<Books> GetBookByISBN(string isbn)
        {
            Books? book = await _context.Books.FirstOrDefaultAsync(b => b.Isbn == isbn);
            return book != null ? book : throw new Exception($"No such book found! Book ISBN No.: {isbn}");
        }
        
        public async Task<List<Books>> GetBooksByGenre(int genreId)
        {
            List<Books> books = await _context.Books.Where(b => b.GenreId == genreId).ToListAsync();
            return books != null ? books : throw new Exception($"No book found for Genre Id: {genreId}");
        }
        
        public async Task<List<Books>> GetBooksByAuthor(string authorName)
        {
            List<Books> books = await _context.Books.Where(b => b.Author == authorName).ToListAsync();
            return books != null ? books : throw new Exception($"No book found for Author: {authorName}");
        }

        public async Task<string> BorrowBook(int bookId, int userId)
        {
            Books? availableBook = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (availableBook == null)
            {
                return $"Incorrect Book Id: {bookId}!";
            }
            if (availableBook.AvailableQuantity < 1)
            {
                return $"Insufficient Book quantity for Book Id: {bookId}!";
            }

            Borrowed? alreadyBorrowed = _context.Borrowed.FirstOrDefault(b => b.BookId == bookId && b.BorrowStatus==Borrow_Status.Borrowed);
            if (alreadyBorrowed != null)
            {
                if (alreadyBorrowed.UserId != userId)
                {
                    return $"Book with Id: {bookId} is already borrowed by someone else!";
                }
                else if (alreadyBorrowed.UserId == userId)
                {
                    return $"Book with Id: {bookId} is already borrowed by you!";
                }
            }

            Reservations? alreadyReserved = _context.Reservations.Where(r => r.BookId == bookId && r.ReservationStatus == Reservation_Status.Pending).FirstOrDefault();
            if (alreadyReserved != null)
            {
                if (alreadyReserved.UserId != userId)
                {
                    return $"Book with Id: {bookId} is unavailable because of existing reservations!";
                }
                else if (alreadyReserved.UserId == userId && alreadyReserved.ReservationStatus==Reservation_Status.Pending)
                {
                    alreadyReserved.ReservationStatus = Reservation_Status.Fullfilled;
                    _context.Reservations.Update(alreadyReserved);
                    await _context.SaveChangesAsync();
                }
                else if (alreadyReserved.UserId == userId && alreadyReserved.ReservationStatus == Reservation_Status.Expired)
                {
                    Reservations? nextReservation = _context.Reservations.FirstOrDefault(r => r.BookId == bookId && r.ReservationStatus == Reservation_Status.Pending);
                    if (nextReservation != null)
                    {
                        return $"Borrow Request Declined. Your Reservation Id: {alreadyReserved.ReservationId} is expired, and reservations by other users exist!";
                    }
                }
            }

            List<Borrowed> totalBorrows = _context.Borrowed.Where(b => b.UserId == userId && b.BorrowStatus==Borrow_Status.Borrowed).ToList();
            if (totalBorrows.Count > 4)
            {
                return "You already have borrowed too many books! Return some and try again.";
            }

            Borrowed borrowed = new Borrowed()
            {
                BookId = bookId,
                UserId = userId,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                ReturnDeadline = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                ReturnDate = null,
                BorrowStatus = Borrow_Status.Borrowed
            };
            _context.Borrowed.Add(borrowed);
            await _context.SaveChangesAsync();

            availableBook.AvailableQuantity -= 1;
            _context.Books.Update(availableBook);
            await _context.SaveChangesAsync();

            int borrowId = borrowed.BorrowId;
            List<Fines> fines = new List<Fines>()
            {
                new() { BorrowId = borrowId, FineType = Fine_Type.Book_Overdue, FineAmount = 0, FinePaidStatus = Fine_Paid_Status.Unpaid, PaidDate = null },
                new() { BorrowId = borrowId, FineType = Fine_Type.Book_Damage, FineAmount = 0, FinePaidStatus = Fine_Paid_Status.Unpaid, PaidDate = null },
                new() { BorrowId = borrowId, FineType = Fine_Type.Book_Lost, FineAmount = 0, FinePaidStatus = Fine_Paid_Status.Unpaid, PaidDate = null }
            };
            _context.Fines.AddRange(fines);
            await _context.SaveChangesAsync();

            Notifications notification = new Notifications()
            {
                UserId = userId,
                MessageType = Notification_Type.Borrowed_Book_Related,
                MessageDescription = $"Borrowed Successfully! Borrow ID: {borrowId}, Book ID: {bookId}",
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return $"Borrowed Successfully! Borrow ID: {borrowId}, Book ID: {bookId}";
        }

        public async Task<string> ReturnBook(int borrowId)
        {
            Borrowed? borrowed = _context.Borrowed.FirstOrDefault(b => b.BorrowId == borrowId);

            if (borrowed == null)
            {
                return $"Borrow Id: {borrowId} does not exist!";
            }
            borrowed.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            borrowed.BorrowStatus = Borrow_Status.Returned;
            _context.Borrowed.Update(borrowed);
            await _context.SaveChangesAsync();

            int bookId = borrowed.BookId;
            Books? returnedBook = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (returnedBook != null)
            {
                returnedBook.AvailableQuantity += 1;
                _context.Books.Update(returnedBook);
                await _context.SaveChangesAsync();
            }

            Reservations? nextReservation = _context.Reservations.FirstOrDefault(r => r.BookId == bookId && r.ReservationStatus==Reservation_Status.Pending);
            if (nextReservation != null)
            {
                nextReservation.ReservationExpiryDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
                _context.Reservations.Update(nextReservation);
                await _context.SaveChangesAsync();

                Notifications availableForBorrow = new Notifications()
                {
                    UserId = nextReservation.UserId,
                    MessageType = Notification_Type.Reservation_Book_Related,
                    MessageDescription = $"Book associated with Reservation Id: {nextReservation.ReservationId} is available for borrowing with reservation expiring tomorrow.",
                    SentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Notifications.Add(availableForBorrow);
                await _context.SaveChangesAsync();
            }

            List<Fines> fines = _context.Fines.Where(f => f.BorrowId == borrowId).ToList();

            foreach( Fines fine in fines)
            {
                fine.FineAmount = 0;
                fine.FinePaidStatus = Fine_Paid_Status.Paid;
                fine.PaidDate = DateOnly.FromDateTime(DateTime.Now);
            }
            _context.Fines.UpdateRange(fines);
            await _context.SaveChangesAsync();

            int userId = borrowed.UserId;
            Notifications notification = new Notifications()
            {
                UserId = userId,
                MessageType = Notification_Type.Borrowed_Book_Related,
                MessageDescription = $"Book Returned Successfully! Borrow Id: {borrowId}, Book Id: {bookId}",
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return $"Book Returned Successfully! Borrow Id: {borrowId}, Book Id: {bookId}";
        }

        public async Task<string> ReportLostBook(int borrowId)
        {
            Borrowed? borrowed = _context.Borrowed.FirstOrDefault(b => b.BorrowId == borrowId);
            if (borrowed == null)
            {
                return $"Borrow Id: {borrowId} does not exist!";
            }

            borrowed.BorrowStatus = Borrow_Status.Lost;
            _context.Borrowed.Update(borrowed);
            await _context.SaveChangesAsync();

            int bookId = borrowed.BookId;
            Books? lostBook = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (lostBook != null)
            {
                lostBook.TotalQuantity -= 1;
                _context.Books.Update(lostBook);
                await _context.SaveChangesAsync();
            }

            List<Fines> fines = _context.Fines.Where(f => f.BorrowId == borrowId).ToList();
            foreach (Fines fine in fines)
            {
                fine.FineAmount = 0;
                fine.FinePaidStatus = Fine_Paid_Status.Paid;
                fine.PaidDate = DateOnly.FromDateTime(DateTime.Now);
            }
            _context.Fines.UpdateRange(fines);
            await _context.SaveChangesAsync();

            int userId = borrowed.UserId;
            Notifications notification = new Notifications()
            {
                UserId = userId,
                MessageType = Notification_Type.Borrowed_Book_Related,
                MessageDescription = $"Book Lost Reported Successfully! Borrow Id: {borrowId}, Book Id: {bookId}",
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return $"Book Lost Reported Successfully! Borrow Id: {borrowId}, Book Id: {bookId}";
        }

        public async Task<string> ReserveBook(int bookId, int userId)
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

            Borrowed? b = _context.Borrowed.FirstOrDefault(b => b.BookId == bookId && b.BorrowStatus==Borrow_Status.Borrowed);
            
            Reservations? r = _context.Reservations.Where(r => r.BookId == bookId && r.ReservationStatus == Reservation_Status.Pending).OrderBy(r => r.ApplicationTimestamp).FirstOrDefault();

            if (r == null)
            {
                if (b == null)
                {
                    reservation.ExpectedAvailabilityDate = DateOnly.FromDateTime(DateTime.Now);
                    reservation.ReservationExpiryDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
                    reservation.ReservationStatus = Reservation_Status.Pending;
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();

                    Notifications notification1 = new Notifications()
                    {
                        UserId = userId,
                        MessageType = Notification_Type.Reservation_Book_Related,
                        MessageDescription = $"Reservation Successful! Reservation Id: {reservation.ReservationId}, Book Id: {bookId}.",
                        SentDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    _context.Notifications.Add(notification1);
                    await _context.SaveChangesAsync();

                    Notifications notification2 = new Notifications()
                    {
                        UserId = userId,
                        MessageType = Notification_Type.Reservation_Book_Related,
                        MessageDescription = $"Your reserved book with Reservation Id: {reservation.ReservationId} is available for borrowing.",
                        SentDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    _context.Notifications.Add(notification2);
                    await _context.SaveChangesAsync();

                    return $"Reservation Successful! Reservation Id: {reservation.ReservationId}. Also, the book is available for borrowing now!";
                }

                if (b != null)
                {
                    if (b.UserId == userId)
                    {
                        return "You cannot reserve a book you have already borrowed!";
                    }

                    reservation.ExpectedAvailabilityDate = b.ReturnDeadline.AddDays(1);
                    reservation.ReservationExpiryDate = null;
                    reservation.ReservationStatus = Reservation_Status.Pending;
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();

                    Notifications notification = new Notifications()
                    {
                        UserId = userId,
                        MessageType = Notification_Type.Reservation_Book_Related,
                        MessageDescription = $"Reservation Successful! Reservation Id: {reservation.ReservationId}, Book Id: {bookId}.",
                        SentDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();
                    return $"Reservation Successful! Reservation Id: {reservation.ReservationId}, Book Id: {bookId}.";
                }
            }

            if (r != null)
            {
                if (r.UserId == userId && r.ReservationStatus==Reservation_Status.Pending)
                {
                    return $"You already have a reservation for this book!";
                }

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
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                Notifications notification = new Notifications()
                {
                    UserId = userId,
                    MessageType = Notification_Type.Reservation_Book_Related,
                    MessageDescription = $"Reservation Successful! Reservation Id: {reservation.ReservationId}, Book Id: {bookId}.",
                    SentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                return $"Reservation Successful! Reservation Id: {reservation.ReservationId}, Book Id: {bookId}.";
            }
            return $"Reservation Unsuccessful! Do not worry, its an error on our side. We will fix it ASAP :)";
        }

        public async Task<string> CancelBookReservation(int reservationId)
        {
            Reservations? reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId);
            if (reservation == null)
            {
                return $"Reservation Id: {reservationId} does not exist";
            }

            reservation.ReservationStatus = Reservation_Status.Cancelled;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return $"Reservation Cancelled Successfully! Reservation Id: {reservationId}";
        }

        public async Task<string> ResetPassword(Reset_Password_DTO reset_Password_DTO)
        {
            Users? existingUser = _context.Users.Where(u => u.UserId == reset_Password_DTO.UserId).FirstOrDefault();
            if (existingUser == null)
            {
                return $"User Id: {reset_Password_DTO.UserId} not found!";
            }

            if (existingUser.PasswordHash != reset_Password_DTO.OldPassword)
            {
                return $"Old password does not match";
            }

            existingUser.PasswordHash = reset_Password_DTO.NewPassword;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            Notifications notification = new Notifications()
            {
                UserId = existingUser.UserId,
                MessageType = Notification_Type.Account_Related,
                MessageDescription = $"Password Successfully Updated!",
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            LogUserActivity logUserActivity = new LogUserActivity()
            {
                UserId = existingUser.UserId,
                ActionType = Action_Type.UpdatedAccount,
                Timestamp = DateTime.Now
            };
            _context.LogUserActivity.Add(logUserActivity);
            await _context.SaveChangesAsync();

            return $"Password Successfully Updated!";
        }
    }
}
