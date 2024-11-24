using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book_Hub_Web_API.Data.DTO;

namespace Book_Hub_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private ICommonRepository _commonRepository;
        public HomeController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }



        [Route("TestAPI")]
        [HttpGet]
        public IActionResult TestAPI()
        {
            return Ok("API Test Successful!");
        }



        [Route("Validate")]
        [HttpPost]
        public IActionResult Validate([FromForm] string username, [FromForm] string password)
        {
            try
            {
                var result = _commonRepository.ValidateUser(username, password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while validating the user.");
            }
        }


        
        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(string name, string email, string phone, string address, string passwordHash)
        {
            await Task.Delay(100);
            Users u = new Users()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Address = address,
                PasswordHash = passwordHash
            };
            await _commonRepository.CreateUser(u);
            return Ok("User created successfully!");
        }



        [Route("UpdateUser")]
        [HttpPatch]
        public async Task<Users> UpdateUser(int userId, string name, string phone, string address)
        {

            Users u = await _commonRepository.UpdateUser(userId, name, phone, address);
            return u;
        }



        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _commonRepository.DeleteUser(userId);
            return Ok("User deleted successfully!");
        }
    }
}
