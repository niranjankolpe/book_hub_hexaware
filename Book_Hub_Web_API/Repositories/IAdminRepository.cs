using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Models;

namespace Book_Hub_Web_API.Repositories
{
    public interface IAdminRepository
    {

        Task<List<Borrowed>> GetAllBorrowed();
        Task<List<Fines>> GetAllFines();
        Task<List<Genres>> GetAllGenres();
        Task<List<LogUserActivity>> GetAllLogUserActivity();
        Task<List<Notifications>> GetAllNotifications();
        Task<List<Reservations>> GetAllReservations();
        Task<List<Users>> GetAllUsers();

        Task<List<ContactUs>> GetAllConsumerQueries();

        Task<ContactUs> AcknowledgeConsumerQuery(int queryId);

        Task<Books> AddBook(Add_Book_DTO add_Book_DTO);
        Task<Books> UpdateBook(Update_book_dto update_Book_Dto);
        Task<Books> RemoveBook(int bookId);
    }
}
