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

        //[Test]
        //public void Test1()
        //{
        //    Assert.Pass();
        //}

        [Test]
        public async Task GetAllBooks_NoParams_ReturnsOkObjectResult()
        {
            List<Books> bookList = new List<Books>()
            {
                new Books(){BookId=1, Isbn="3226", Title="A Journey to the Past", Cost=47.99M, AvailableQuantity=5, TotalQuantity=5, GenreId=2},
                new Books(){BookId=2, Isbn="857483", Title="A Trip to Paris", Cost=94.99M, AvailableQuantity=17, TotalQuantity=17, GenreId=8}
            };

            _commonRepository.Setup(repo => repo.GetAllBooks()).ReturnsAsync(bookList);

            var result = await _homeController.GetAllBooks();
            var okObjectResult = result as OkObjectResult;
            Assert.That(200, Is.EqualTo(okObjectResult?.StatusCode));
        }

        [Test]
        public async Task GetAllBooks_NoParams_ReturnsBadRequestObjectResult()
        {
            List<Books> bookList = new List<Books>()
            {

            };

            var exceptionMessage = "No book found!";
            _commonRepository.Setup(repo => repo.GetAllBooks()).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _homeController.GetAllBooks();
            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.That(400, Is.EqualTo(badRequestObjectResult?.StatusCode));
            Assert.That(exceptionMessage, Is.EqualTo(badRequestObjectResult?.Value));
        }
    }
}