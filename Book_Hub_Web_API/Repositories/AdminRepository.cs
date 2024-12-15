using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Data;
using Microsoft.EntityFrameworkCore;
using Book_Hub_Web_API.Data.DTO;
using System.Net;
using Book_Hub_Web_API.Data.Enums;

namespace Book_Hub_Web_API.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private BookHubDBContext _DBContext;

        public AdminRepository(BookHubDBContext dBContext)
        {
            _DBContext = dBContext;
        }

        public async Task<Books> AddBook(Add_Book_DTO add_Book_DTO)
        {
            try
            {
                Books book = new Books()
                {
                    Isbn = add_Book_DTO.Isbn,
                    Title = add_Book_DTO.Title,
                    Author = add_Book_DTO.Author,
                    Publication = add_Book_DTO.Publication,
                    PublishedDate = add_Book_DTO.PublishedDate,
                    Edition = add_Book_DTO.Edition,
                    Language = add_Book_DTO.Language,
                    Description = add_Book_DTO.Description,
                    Cost = add_Book_DTO.Cost,
                    AvailableQuantity = add_Book_DTO.AvailableQuantity,
                    TotalQuantity = add_Book_DTO.TotalQuantity,
                    GenreId = add_Book_DTO.GenreId

                };

                //adding the books to Books dbset and saving changes
                await _DBContext.Books.AddAsync(book);
                await _DBContext.SaveChangesAsync();
                return book;
            }
            catch(Exception e)
            {
                throw new Exception("Cannot Be Added");
            }
        }


        public async Task<List<Borrowed>> GetAllBorrowed()
        {

            try
            {
                return await _DBContext.Borrowed.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving borrowed records: {ex.Message}", ex);
            }
        }


        public async Task<List<Fines>> GetAllFines()
        {
            try
            {
                return await _DBContext.Fines.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving borrowed records: {ex.Message}", ex);
            }
        }



        public async Task<List<Genres>> GetAllGenres()
        {
            try
            {
                return await _DBContext.Genres.ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving borrowed records: {ex.Message}", ex);
            }

        }



        public async Task<List<LogUserActivity>> GetAllLogUserActivity()
        {
            try
            {
                return await _DBContext.LogUserActivity.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving borrowed records: {ex.Message}", ex);
            }

        }



        public async Task<List<Notifications>> GetAllNotifications()
        {
            try
            {
                return await _DBContext.Notifications.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving borrowed records: {ex.Message}", ex);
            }

        }



        public async Task<List<Reservations>> GetAllReservations()
        {
            try
            {
                return await _DBContext.Reservations.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving borrowed records: {ex.Message}", ex);
            }

        }



        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                return await _DBContext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving borrowed records: {ex.Message}", ex);
            }

        }

        public async Task<List<ContactUs>> GetAllConsumerQueries()
        {
            List<ContactUs> queries = await _DBContext.ContactUs.ToListAsync();
            if (queries.Count < 1)
            {
                throw new Exception("No consumer queries found!");
            }
            return queries;
        }

        public async Task<ContactUs> AcknowledgeConsumerQuery(int queryId)
        {
            ContactUs query = await _DBContext.ContactUs.FirstAsync(c => c.QueryId == queryId);
            if (query == null)
            {
                throw new Exception($"No consumer query with Query ID: {queryId} found!");
            }
            query.Query_Status = Contact_Us_Query_Status.Acknowledged;

            _DBContext.ContactUs.Update(query);
            await _DBContext.SaveChangesAsync();
            
            return query;
        }

        public async Task<Books> RemoveBook(int bookId)
        {

            try
            {

                var book = await _DBContext.Books.Where(b => b.BookId == bookId).FirstOrDefaultAsync();
                if (book == null)
                {
                    throw new KeyNotFoundException("Book not found.");
                }

                book.AvailableQuantity = 0;
                book.TotalQuantity = 0;


                // Update the book record in the context
                _DBContext.Books.Update(book);

                // Save changes asynchronously to the database
                await _DBContext.SaveChangesAsync();

                return book;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing book: {ex.Message}", ex);
            }
        }


        public async Task<Books> UpdateBook(Update_book_dto update_Book_Dto)
        {
            try
            {
                var book = await _DBContext.Books
                                          .Where(b => b.BookId == update_Book_Dto.BookId)
                                          .FirstOrDefaultAsync();
                if (book != null)
                {
                    book.AvailableQuantity = update_Book_Dto.AvailableQuantity;
                    book.TotalQuantity = update_Book_Dto.TotalQuantity;

                    _DBContext.Books.Update(book);

                    await _DBContext.SaveChangesAsync();
                    return book;
                }
                return book!=null ? book :new Books();

            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating book: {ex.Message}", ex);
            }
        }
    }
}
