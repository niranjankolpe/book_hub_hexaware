using System.Net;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book_Hub_Web_API.Data.DTO;

namespace Book_Hub_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("GetBookByBookId")]
        [HttpPost]
        public async Task<IActionResult> GetBookByBookId(int bookId)
        {
            await Task.Delay(100);
            Books book = await _userRepository.GetBookByBookId(bookId);
            return Ok(new JsonResult(book));
        }

        [Route("GetBookByISBN")]
        [HttpPost]
        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            await Task.Delay(100);
            Books book = await _userRepository.GetBookByISBN(isbn);
            return Ok(new JsonResult(book));
        }

        [Route("GetBooksByGenre")]
        [HttpPost]
        public async Task<IActionResult> GetBooksByGenre(int genreId)
        {
            await Task.Delay(100);
            List<Books> books = await _userRepository.GetBooksByGenre(genreId);
            return Ok(new JsonResult(books));
        }

        [Route("GetBooksByAuthor")]
        [HttpPost]
        public async Task<IActionResult> GetBooksByAuthor(string authorName)
        {
            await Task.Delay(100);
            List<Books> books = await _userRepository.GetBooksByAuthor(authorName);
            return Ok(new JsonResult(books));
        }

        [Route("BorrowBook")]
        [HttpPost]
        public async Task<IActionResult> BorrowBook(int bookId, int userId)
        {
            await Task.Delay(100);
            Borrowed borrowed = await _userRepository.BorrowBook(bookId, userId);
            return Ok(new JsonResult(borrowed));
        }

        [Route("ReturnBook")]
        [HttpPatch]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            await Task.Delay(100);
            Borrowed borrowed = await _userRepository.ReturnBook(borrowId);
            return Ok(new JsonResult(borrowed));
        }

        [Route("ReportLostBook")]
        [HttpPatch]
        public async Task<IActionResult> ReportLostBook(int borrowId)
        {
            await Task.Delay(100);
            Borrowed borrowed = await _userRepository.ReportLostBook(borrowId);
            return Ok(new JsonResult(borrowed));
        }

        [Route("ReserveBook")]
        [HttpPost]
        public async Task<IActionResult> ReserveBook(int bookId, int userId)
        {
            await Task.Delay(100);
            Reservations reservation = await _userRepository.ReserveBook(bookId, userId);
            return Ok(new JsonResult(reservation));
        }

        [Route("CancelBookReservation")]
        [HttpPatch]
        public async Task<IActionResult> CancelBookReservation(int reservationId)
        {
            await Task.Delay(100);
            Reservations reservation = await _userRepository.CancelBookReservation(reservationId);
            return Ok(new JsonResult(reservation));
        }

        [Route("ResetPassword")]
        [HttpPatch]
        public async Task<IActionResult> ResetPassword([FromForm] Reset_Password_DTO reset_password_dto)
        {
            await _userRepository.ResetPassword(reset_password_dto);
            return Ok("Password Reset Successfully");
        }
    }
}
