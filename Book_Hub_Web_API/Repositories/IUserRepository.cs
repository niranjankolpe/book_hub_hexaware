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

        Task<string> BorrowBook(int bookId, int userId);

        Task<string> ReturnBook(int borrowId);

        Task<string> ReportLostBook(int borrowId);

        Task<string> ReserveBook(int bookId, int userId);

        Task<string> CancelBookReservation(int reservationId);

        Task<string> ResetPassword(Reset_Password_DTO reset_Password_DTO);
    }
}
