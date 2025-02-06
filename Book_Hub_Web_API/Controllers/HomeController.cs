using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book_Hub_Web_API.Data.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Book_Hub_Web_API.Services;
using Book_Hub_Web_API.Data.Enums;
using System.Diagnostics;
using log4net;
using static System.Net.WebRequestMethods;

namespace Book_Hub_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private ICommonRepository _commonRepository;

        private readonly IConfiguration _configuration;

        private IEmailService _emailService;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));

        public HomeController(ICommonRepository commonRepository, IConfiguration configuration, IEmailService emailService)
        {
            _commonRepository = commonRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        [Route("GetAllBooks")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                _logger.Info($"Called:{nameof(GetAllBooks)}");
                var books = await _commonRepository.GetAllBooks();
                //_logger.Debug(nameof(GetAllBooks));
                return Ok(books);
            }
            catch(Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("GenerateOTP")]
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GenerateOTP([FromForm] [Bind("emailAddress")] string emailAddress)
        {
            try
            {
                _logger.Info($"Called:{nameof(GenerateOTP)}");
                Random random = new Random();
                string otp = random.Next(100000, 999999).ToString();
                Debug.WriteLine("\n\nGot email address to send mail is: ", emailAddress, "\n\n");

                _emailService.SendEmail([emailAddress], Notification_Type.Account_Related.ToString(), $"Your OTP for Book Hub Platform is: {otp}");

                return new JsonResult(new { value = otp });
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return new JsonResult(new { value = "Error" });
            }
        }


        [Route("Validate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Validate([FromForm][Bind("Email", "PasswordHash")] Validate_User_DTO validate_User_DTO)
        {
            try
            {
                _logger.Info($"Called:{nameof(Validate)}");
                if (ModelState.IsValid)
                {
                    Users result = await _commonRepository.ValidateUser(validate_User_DTO);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Invalid input format!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm][Bind("Email", "PasswordHash")] Validate_User_DTO validate_User_DTO)
        {
            try
            {
                _logger.Info($"Called:{nameof(Login)}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Users user = await _commonRepository.ValidateUser(validate_User_DTO);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim ("UserId", user.UserId.ToString()),
                    new Claim ("Email",  user.Email.ToString()),
                    new Claim (ClaimTypes.Role, user.Role.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signInCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: signInCreds
                );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                await _commonRepository.Login(user.UserId);

                return Ok(tokenValue);
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("Logout")]
        [HttpPost]
        [Authorize(Roles = "Consumer,Administrator")]
        public async Task<IActionResult> Logout([FromForm]int userId)
        {
            try
            {
                _logger.Info($"Called:{nameof(Logout)}");
                await _commonRepository.Logout(userId);
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm][Bind("Name", "Email", "Phone", "Address", "PasswordHash")] Create_User_DTO create_User_DTO)
        {
            try
            {
                _logger.Info($"Called:{nameof(CreateUser)}");
                if (ModelState.IsValid)
                {
                    await Task.Delay(100);

                    var user = await _commonRepository.CreateUser(create_User_DTO);
                    _emailService.SendEmail([user.Email], Notification_Type.Account_Related.ToString(), $"Welcome to Book Hub, Dear {user.Name}");
                    return Ok(new JsonResult(user));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("UpdateUser")]
        [HttpPatch]
        [Authorize(Roles = "Consumer,Administrator")]
        public async Task<IActionResult> UpdateUser( [FromForm][Bind("UserId", "Name", "Phone", "Address")] UpdateUser_DTO updateUser_DTO)
        {
            try
            {
                _logger.Info($"Called:{nameof(UpdateUser)}");
                if (ModelState.IsValid)
                {
                    Users u = await _commonRepository.UpdateUser(updateUser_DTO);
                    _emailService.SendEmail([u.Email], Notification_Type.Account_Related.ToString(), $"Dear {u.Name}, Successfully Updated your Account details at Book Hub!");
                    return Ok(u);
                }
                else
                {
                    return BadRequest("One or more input values are invalid!");
                }
            }
            catch(Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }


        [Route("DeleteUser")]
        [HttpPost]
        [Authorize(Roles = "Consumer,Administrator")]
        public async Task<IActionResult> DeleteUser([FromForm] int userId)
        {
            try
            {
                _logger.Info($"Called:{nameof(DeleteUser)}");
                var temporaryUser = await _commonRepository.DeleteUser(userId);
                _emailService.SendEmail([temporaryUser.Email], Notification_Type.Account_Related.ToString(), $"Dear {temporaryUser.Name},\n\nSuccessfully Deleted your Account details at Book Hub!\n\nSorry to see you go :(\n\nRegards,\nBook Hub");
                return Ok(new JsonResult(temporaryUser));
            }
            catch(Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("ForgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromForm] string emailAddress, [FromForm] string newPassword)
        {
            try
            {
                _logger.Info($"Called:{nameof(ForgotPassword)}");
                var user = await _commonRepository.ForgotPassword(emailAddress, newPassword);
                _emailService.SendEmail([emailAddress], Notification_Type.Account_Related.ToString(), $"Your password was Updated Successfully!");
                return Ok(new JsonResult(value: "Password successfully Updated!"));
            }
            catch(Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("AddContactUsQuery")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddContactUsQuery([FromForm][Bind("Email", "Query_Type", "Description")] Contact_Us_DTO contact_Us_DTO)
        {
            try
            {
                _logger.Info($"Called:{nameof(AddContactUsQuery)}");
                var contact_us = await _commonRepository.AddContactUsQuery(contact_Us_DTO);
                return Ok(new JsonResult(value: "Query submitted successfully!"));
            }
            catch(Exception ex)
            {
                _logger.Error($"{ex.GetType().Name}:{ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
