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

        [Route("TestAPI")]
        [HttpGet]
        public IActionResult TestAPI ()
        {
            return Ok("API Test Successful!");
        }
    }
}
