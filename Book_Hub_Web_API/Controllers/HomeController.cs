using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
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

        [Route("Get")]
        [HttpGet]
        public ActionResult<List<Users>> GetUsers()
        {
            var users = _commonRepository.GetAllBooks();
            return Ok(users);
        }
        /*
         * methods after connecting to database
         * [HttpGet]
         * public async Task<ActionResult<List<Books>>> GetAllBooks(){
         *   try{
         *   var books =  await _commonRepository.GetAllBooks();
         *   return Ok(books);
         *   }
         *   catch(exception e){
         *   return StatusCode(500,"Internal Server Error");
         *   }
         * // POST: api/users/validate
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateUser([FromBody] User loginUser)
    {
        try
        {
            // Call the repository method to validate the user
            var user = await _userRepository.ValidateUserAsync(loginUser.Username, loginUser.Password);

            if (user == null)
            {
                // User not found or password incorrect
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Return success with user data (excluding sensitive information like password)
            return Ok(new { message = "User authenticated", user = new { user.Id, user.Username, user.FullName, user.Email } });
        }
        catch (Exception ex)
        {
            // Handle unexpected errors
            return StatusCode(500, "Internal server error");
        }
    }
         * 
         * */

       

        [Route("CreateUser")]
        [HttpPost]
        public async Task<Users> CreateUser([Bind("UserId", "Name", "Email", "Phone", "Address", "PasswordHash")] Users user)
        {
            await _commonRepository.CreateUser(user);
            return user;
        }

        [Route("UpdateUser")]
        [HttpPatch]
        public async Task<Users> UpdateUser(Users user)
        {
            await _commonRepository.UpdateUser(user);
            return user;
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
