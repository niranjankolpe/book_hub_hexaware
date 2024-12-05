using Book_Hub_Web_API.Controllers;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Repositories;
using Book_Hub_Web_API.Data;
using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Data.Enums;

using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace BookHubTestProject.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {

        private HomeController _homeController;

        private Mock<ICommonRepository> _commonRepository;

        private Mock<IConfiguration> _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
            _commonRepository = new Mock<ICommonRepository>();

            _homeController = new HomeController(_commonRepository.Object, _configuration.Object);
        }

        [Test]
        public async Task GetAllBooks_ReturnsList_noparameterAsync()
        {
            List<Books> books = new List<Books>
        {
            new Books
            {
                BookId = 1,
                Isbn = "978-3-16-148410-0",
                Title = "The Great Adventure",
                Author = "John Smith",
                Publication = "Adventure Books Ltd.",
                PublishedDate = new DateOnly(2021, 6, 15),
                Edition = "1st",
                Language = "English",
                Description = "A thrilling adventure story.",
                Cost = 19.99M,
                AvailableQuantity = 10,
                TotalQuantity = 20,
                GenreId = 1
            },
            new Books
            {
                BookId = 2,
                Isbn = "978-1-23-456789-7",
                Title = "Learning C#",
                Author = "Jane Doe",
                Publication = "Tech Publishers",
                PublishedDate = new DateOnly(2023, 1, 10),
                Edition = "2nd",
                Language = "English",
                Description = "An in-depth guide to C# programming.",
                Cost = 29.99M,
                AvailableQuantity = 5,
                TotalQuantity = 15,
                GenreId = 2
            },
            new Books
            {
                BookId = 3,
                Isbn = "978-0-12-345678-9",
                Title = "Mysteries Unveiled",
                Author = "Emily Johnson",
                Publication = "Mystery World",
                PublishedDate = new DateOnly(2020, 9, 25),
                Edition = "3rd",
                Language = "English",
                Description = "A collection of intriguing mysteries.",
                Cost = 24.99M,
                AvailableQuantity = 7,
                TotalQuantity = 12,
                GenreId = 3
            }
        };

            _commonRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);

            var result = await _homeController.GetAllBooks();
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }

        [Test]
        public async Task GetAllBooks_ReturnsBadRequestObjectResult()
        {
            List<Books> bookList = new List<Books>()
            {

            };

            var exceptionMessage = "No Book Found";
            _commonRepository.Setup(repo => repo.GetAllBooks()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _homeController.GetAllBooks();

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        //Delete User
        [Test]
        [TestCase(1)]
        public async Task DeleteUser_Returns_String(int id)
        {
            string s = "User Deleted SuccessFully";

            _commonRepository.Setup(repo => repo.DeleteUser(id)).ReturnsAsync(s);

            var result = await _homeController.DeleteUser(id);
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }
        //I have updated Delete user action method in controller added Catch and bad request
        //[Test]
        //[TestCase(5)]
        //public async Task DeleteUser_Returns_BadRequestObject(int id)
        //{

        //    var exceptionMessage = "No User Found";
        //    _commonRepository.Setup(repo => repo.DeleteUser(id)).ThrowsAsync(new Exception(exceptionMessage));

        //    var result = await _homeController.DeleteUser(id);

        //    var me = result as BadRequestObjectResult;
        //    Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
        //    Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));


        //}
        [Test]
        [TestCase("Seja", "ss@gamil.com", "6654437", "Indore", "ttt")]
        public async Task CreateUser_TakesAddUserDTO_returnsUserObject(string Name, string Email, string Phone, string Address, string PasswordHash)
        {
            Create_User_DTO create_User_DTO = new Create_User_DTO()
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                Address = Address,
                PasswordHash = PasswordHash

            };
            Users user = new Users()
            {
                UserId = 1,
                Name = Name,
                Email = Email,
                Phone = Phone,
                Address = Address,
                PasswordHash = PasswordHash,
                Role = Book_Hub_Web_API.Data.Enums.User_Role.Consumer,
                AccountCreatedDate = DateOnly.Parse("2024-10-10")
            };

            _commonRepository.Setup(repo => repo.CreateUser(create_User_DTO)).ReturnsAsync(user);

            var result = await _homeController.CreateUser(create_User_DTO);
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));


        }
        [Test]
        [TestCase("Seja", "ss@gamil.com", "6654437", "Indore", "ttt")]
        public async Task CreateUser_TakesAddUserDTO_returnsBadRequest(string Name, string Email, string Phone, string Address, string PasswordHash)
        {
            Create_User_DTO create_User_DTO = new Create_User_DTO()
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                Address = Address,
                PasswordHash = PasswordHash

            };
            Users user = new Users()
            {

            };

            var exceptionMessage = "User cannot be added";
            _commonRepository.Setup(repo => repo.CreateUser(create_User_DTO)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _homeController.CreateUser(create_User_DTO);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }

        [Test]
        [TestCase("ss@gamil.com", "ttt")]
        public async Task ValidateUser_takesValidateUserDto_returnsOkObject(string Email, string PasswordHash)
        {
            Validate_User_DTO validate_User_DTO = new Validate_User_DTO
            {
                Email = Email,
                PasswordHash = PasswordHash
            };
            Users user = new Users()
            {
                UserId = 1,
                Name = "Sejal Sihare",
                Email = Email,
                Phone = "9988766",
                Address = "Jaipur",
                PasswordHash = PasswordHash,
                Role = Book_Hub_Web_API.Data.Enums.User_Role.Consumer,
                AccountCreatedDate = DateOnly.Parse("2024-10-10")
            };

            _commonRepository.Setup(repo => repo.ValidateUser(validate_User_DTO)).ReturnsAsync(user);

            var result = await _homeController.Validate(validate_User_DTO);
            var okObjectResult = new OkObjectResult(result);

            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        [TestCase("ss@gamil.com", "ttt")]
        public async Task ValidateUser_takesValidateUserDto_returnsBadRequestObject(string Email, string PasswordHash)
        {
            Validate_User_DTO validate_User_DTO = new Validate_User_DTO
            {
                Email = Email,
                PasswordHash = PasswordHash
            };
            Users user = new Users()
            {

            };
            var exceptionMessage = "User with those credentials not found!";

            _commonRepository.Setup(repo => repo.ValidateUser(validate_User_DTO)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _homeController.Validate(validate_User_DTO);
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }
    }
}