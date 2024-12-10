using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Models;

namespace Book_Hub_Web_API.Repositories
{
    public interface IUserRepository
    {
        Task<Books> GetBookByBookId(int bookId);

        Task<Books> GetBookByISBN(string isbn);

        Task<List<Books>> GetBooksByGenre(int genreId);

        Task<List<Books>> GetBooksByAuthor(string authorName);

        Task<Borrowed> BorrowBook(int bookId, int userId);

        Task<Borrowed> ReturnBook(int borrowId);

        Task<Borrowed> ReportLostBook(int borrowId);

        Task<Reservations> ReserveBook(int bookId, int userId);

        Task<Reservations> CancelBookReservation(int reservationId);

        Task<Users> ResetPassword(Reset_Password_DTO reset_Password_DTO);

        Task<List<Notifications>> GetNotificationsByUserId(int userId);

        Task<List<Reservations>> GetReservationsByUserId(int userId);

        Task<List<Borrowed>> GetBorrowedByUserId(int userId);

        //Task<List<Fines>> GetFinesByUserId(int userId);
    }
}
