using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_Hub_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;

        }

        [HttpPost]
        [Route("AddBook")]
        public async Task<ActionResult<Books>> AddBooks([FromForm] [Bind(" Isbn", "Title", "Author", "Publication", "PublishedDate", "Edition", "Language", "Description", "Cost", "AvailableQuantity", "TotalQuantity", "GenreId")] Add_Book_DTO add_Book_DTO)
        {
            
            try
            {
                var books = await _adminRepository.AddBook(add_Book_DTO);
                return Ok(books);

            }
            catch (Exception ex)
            {
                // Log the error (optional) and return 500 Internal Server Error
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }

        }



        [HttpGet]
        [Route("GetBorrowed")]
        public async Task<ActionResult<List<Borrowed>>> GetAllBorrowed()
        {
            try
            {

                var borrowedRecords = await _adminRepository.GetAllBorrowed();

                // If no records were found, return a 404 response with a message
                if (borrowedRecords == null || borrowedRecords.Count == 0)
                {
                    return NotFound(new { message = "No borrowed records found." });
                }
                return Ok(borrowedRecords);
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a 500 error
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }

        }



        [HttpGet]
        [Route("GetFines")]
        public async Task<ActionResult<List<Fines>>> GetAllFine()
        {
            try
            {
                var finerecord = await _adminRepository.GetAllFines();

                if (finerecord == null || finerecord.Count == 0)
                {
                    return NotFound(new { message = "No  Fines records found." });
                }
                return Ok(finerecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetGenre")]
        public async Task<ActionResult<List<Genres>>> GetAllGenre()
        {
            try
            {
                var genrerecord = await _adminRepository.GetAllGenres();

                if (genrerecord == null || genrerecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(genrerecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetLog")]
        public async Task<ActionResult<List<LogUserActivity>>> GetAllLogUserActivity()
        {
            try
            {
                var logrecord = await _adminRepository.GetAllLogUserActivity();

                if (logrecord == null || logrecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(logrecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetNotification")]
        public async Task<ActionResult<List<Notifications>>> GetAllNotification()
        {
            try
            {
                var notirecord = await _adminRepository.GetAllNotifications();

                if (notirecord == null || notirecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(notirecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetReservation")]
        public async Task<ActionResult<List<Reservations>>> GetAllReservation()
        {
            try
            {
                var reservrecord = await _adminRepository.GetAllReservations();

                if (reservrecord == null || reservrecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(reservrecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetUser")]
        public async Task<ActionResult<List<Users>>> GetAllUser()
        {
            try
            {
                var userrecord = await _adminRepository.GetAllUsers();

                if (userrecord == null || userrecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(userrecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [Route("RemoveBook")]
        [HttpPatch]
        public async Task<ActionResult<Books>> RemoveBook([FromForm]int bookid)
        {
            try
            {

                var book = await _adminRepository.RemoveBook(bookid);
                if (book == null)
                {
                    return NotFound(new { message = "Book not found." });
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });

            }
        }



        [HttpPatch]
        [Route("Updatebook")]
        public async Task<ActionResult<Books>> UpdateBooks([FromForm] [Bind("BookId", "AvailableQuantity", "TotalQuantity")] Update_book_dto update_Book_Dto)
        {
            try
            {
                var book = await _adminRepository.UpdateBook(update_Book_Dto);

                if (book != null)
                {
                    return Ok(book);
                }

                return NotFound(new { message = "Book not found." });

            }
            catch (Exception ex)
            {
              return  StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
