using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book_Hub_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    //[AllowAnonymous]
    public class AdminController : ControllerBase
    {
        private IAdminRepository _adminRepository;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(AdminController));

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> AddBooks([FromForm] [Bind(" Isbn", "Title", "Author", "Publication", "PublishedDate", "Edition", "Language", "Description", "Cost", "AvailableQuantity", "TotalQuantity", "GenreId")] Add_Book_DTO add_Book_DTO)
        {
            try
            {
                _logger.Info($"Called:{nameof(AddBooks)}");
                var books = await _adminRepository.AddBook(add_Book_DTO);
                return Ok(books);

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
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
                _logger.Info($"Called:{nameof(GetAllBorrowed)}");
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
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
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
                _logger.Info($"Called:{nameof(GetAllFine)}");
                var finerecord = await _adminRepository.GetAllFines();

                if (finerecord == null || finerecord.Count == 0)
                {
                    return NotFound(new { message = "No  Fines records found." });
                }
                return Ok(finerecord);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetGenre")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Genres>>> GetAllGenre()
        {
            try
            {
                _logger.Info($"Called:{nameof(GetAllGenre)}");
                var genrerecord = await _adminRepository.GetAllGenres();

                if (genrerecord == null || genrerecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(genrerecord);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetLog")]
        public async Task<ActionResult<List<LogUserActivity>>> GetAllLogUserActivity()
        {
            try
            {
                _logger.Info($"Called:{nameof(GetAllLogUserActivity)}");
                var logrecord = await _adminRepository.GetAllLogUserActivity();

                if (logrecord == null || logrecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(logrecord);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetNotification")]
        public async Task<ActionResult<List<Notifications>>> GetAllNotification()
        {
            try
            {
                _logger.Info($"Called:{nameof(GetAllNotification)}");
                var notirecord = await _adminRepository.GetAllNotifications();

                if (notirecord == null || notirecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(notirecord);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetReservation")]
        public async Task<ActionResult<List<Reservations>>> GetAllReservation()
        {
            try
            {
                _logger.Info($"Called:{nameof(GetAllReservation)}");
                var reservrecord = await _adminRepository.GetAllReservations();

                if (reservrecord == null || reservrecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(reservrecord);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpGet]
        [Route("GetUser")]
        public async Task<ActionResult<List<Users>>> GetAllUser()
        {
            try
            {
                _logger.Info($"Called:{nameof(GetAllUser)}");
                var userrecord = await _adminRepository.GetAllUsers();

                if (userrecord == null || userrecord.Count == 0)
                {
                    return NotFound(new { message = "No  Genres records found." });
                }
                return Ok(userrecord);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [Route("GetAllConsumerQueries")]
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllConsumerQueries()
        {
            try
            {
                _logger.Info($"Called:{nameof(GetAllConsumerQueries)}");
                var queries = await _adminRepository.GetAllConsumerQueries();
                return Ok(queries);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [Route("AcknowledgeConsumerQuery")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AcknowledgeConsumerQuery([FromForm]int queryId)
        {
            try
            {
                _logger.Info($"Called:{nameof(AcknowledgeConsumerQuery)}");
                var query = await _adminRepository.AcknowledgeConsumerQuery(queryId);
                return Ok(query);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [Route("RemoveBook")]
        [HttpPatch]
        public async Task<ActionResult<Books>> RemoveBook([FromForm]int bookid)
        {
            try
            {
                _logger.Info($"Called:{nameof(RemoveBook)}");
                var book = await _adminRepository.RemoveBook(bookid);
                if (book == null)
                {
                    return NotFound(new { message = "Book not found." });
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



        [HttpPatch]
        [Route("Updatebook")]
        public async Task<ActionResult<Books>> UpdateBooks([FromForm] [Bind("BookId", "AvailableQuantity", "TotalQuantity")] Update_book_dto update_Book_Dto)
        {
            try
            {
                _logger.Info($"Called:{nameof(UpdateBooks)}");
                var book = await _adminRepository.UpdateBook(update_Book_Dto);

                if (book != null)
                {
                    return Ok(book);
                }

                return NotFound(new { message = "Book not found." });

            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return  StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
