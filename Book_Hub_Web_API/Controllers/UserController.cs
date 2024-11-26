﻿using System.Net;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book_Hub_Web_API.Data.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Book_Hub_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Consumer")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("GetBookByBookId")]
        [HttpPost]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> GetBookByBookId([FromForm]int bookId)
        {
            try
            {
                var book = await _userRepository.GetBookByBookId(bookId);
                return Ok(new JsonResult(book));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetBookByISBN")]
        [HttpPost]
        public async Task<IActionResult> GetBookByISBN([FromForm] string isbn)
        {
            try
            {
                var book = await _userRepository.GetBookByISBN(isbn);
                return Ok(new JsonResult(book));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetBooksByGenre")]
        [HttpPost]
        public async Task<IActionResult> GetBooksByGenre([FromForm] int genreId)
        {
            try
            {
                var books = await _userRepository.GetBooksByGenre(genreId);
                return Ok(new JsonResult(books));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetBooksByAuthor")]
        [HttpPost]
        public async Task<IActionResult> GetBooksByAuthor([FromForm] string authorName)
        {
            try
            {
                var books = await _userRepository.GetBooksByAuthor(authorName);
                return Ok(new JsonResult(books));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("BorrowBook")]
        [HttpPost]
        public async Task<IActionResult> BorrowBook([FromForm] int bookId, [FromForm] int userId)
        {
            try
            {
                var borrowed = await _userRepository.BorrowBook(bookId, userId);
                return Ok(new JsonResult(borrowed));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("ReturnBook")]
        [HttpPatch]
        public async Task<IActionResult> ReturnBook([FromForm] int borrowId)
        {
            try
            {
                var borrowed = await _userRepository.ReturnBook(borrowId);
                return Ok(new JsonResult(borrowed));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("ReportLostBook")]
        [HttpPatch]
        public async Task<IActionResult> ReportLostBook([FromForm] int borrowId)
        {
            try
            {
                var borrowed = await _userRepository.ReportLostBook(borrowId);
                return Ok(new JsonResult(borrowed));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("ReserveBook")]
        [HttpPost]
        public async Task<IActionResult> ReserveBook([FromForm] int bookId, [FromForm] int userId)
        {
            try
            {
                var reservation = await _userRepository.ReserveBook(bookId, userId);
                return Ok(new JsonResult(reservation));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("CancelBookReservation")]
        [HttpPatch]
        public async Task<IActionResult> CancelBookReservation([FromForm] int reservationId)
        {
            try
            {
                var reservation = await _userRepository.CancelBookReservation(reservationId);
                return Ok(new JsonResult(reservation));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("ResetPassword")]
        [HttpPatch]
        public async Task<IActionResult> ResetPassword([FromForm] [Bind("UserId", "Email", "OldPassword", "NewPassword")] Reset_Password_DTO reset_password_dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userRepository.ResetPassword(reset_password_dto);
                    return Ok(new JsonResult(user));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
