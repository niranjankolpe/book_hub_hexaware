using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book_Hub_Web_API.Data.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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



        //[Route("Validate")]
        //[HttpPost]
        //public IActionResult Validate([FromForm] [Bind("Email", "PasswordHash")] Validate_User_DTO validate_User_DTO)
        //{
        //    try
        //    {
        //        var result = _commonRepository.ValidateUser(validate_User_DTO);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        return StatusCode(500, $"An error occurred while validating the user.{ex.Message}");
        //    }
        //}
        [Route("Validate")]
        [HttpPost]
        public async Task<IActionResult> Validate([FromForm] Validate_User_DTO validate_User_DTO)
        {
            try
            {
                // Validate the user via the repository
                string result = await _commonRepository.ValidateUser(validate_User_DTO);

                // Return appropriate response
                if (result == "User is valid")
                {
                    return Ok(new { success = true, message = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = result });
                }
            }
            catch (Exception ex)
            {
                // Log the error with additional details for debugging
                Console.Error.WriteLine($"Error validating user: {ex}");

                // Return a generic error response
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while validating the user. Please try again later."
                });
            }
        }




        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm][Bind("Name", "Email", "Phone", "Address", "PasswordHash")] Create_User_DTO create_User_DTO)
        {
            if (ModelState.IsValid)
            {
                await Task.Delay(100);

                await _commonRepository.CreateUser(create_User_DTO);
                return Ok("User created successfully!");
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
        public async Task<IActionResult> DeleteUser(int userId)
        {
           var result =  await _commonRepository.DeleteUser(userId);
            return Ok(result);
        }
    }
}
