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

namespace Book_Hub_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Consumer,Administrator")]
    public class HomeController : ControllerBase
    {
        private ICommonRepository _commonRepository;

        private readonly IConfiguration _configuration;

        public HomeController(ICommonRepository commonRepository, IConfiguration configuration)
        {
            _commonRepository = commonRepository;
            _configuration = configuration;
        }

        [Route("GetAllBooks")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _commonRepository.GetAllBooks();
                return Ok(books);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Validate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Validate([FromForm] Validate_User_DTO validate_User_DTO)
        {
            try
            {
                Users result = await _commonRepository.ValidateUser(validate_User_DTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
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
                return Ok(tokenValue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm][Bind("Name", "Email", "Phone", "Address", "PasswordHash")] Create_User_DTO create_User_DTO)
        {
            if (ModelState.IsValid)
            {
                await Task.Delay(100);

                var user = await _commonRepository.CreateUser(create_User_DTO);
                return Ok($"User created successfully with User Id: {user.UserId}");
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("UpdateUser")]
        [HttpPatch]
        public async Task<Users> UpdateUser( [FromForm][Bind("UserId", "Name", "Phone", "Address")] UpdateUser_DTO updateUser_DTO)
        {

            Users u = await _commonRepository.UpdateUser(updateUser_DTO);
            return u;
        }


        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromForm] int userId)
        {

           var result =  await _commonRepository.DeleteUser(userId);
            return Ok(result);
        }
    }
}
