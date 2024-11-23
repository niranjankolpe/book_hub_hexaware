using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Book_Hub_Web_API.Repositories
{
    public class CommonRepository : ICommonRepository
    {


        private List<Books> _books = new List<Books>
        {
    new Books(){BookId = 1,Isbn =" 222",Title="The Sage" ,Author = "Rabindra",Publication = "yy",PublishedDate = new DateOnly(2024, 11, 11),Edition = "second",Language = "Hindi",Description ="Journey of Human",Cost =200.99M,AvailableQuantity = 55,TotalQuantity = 100,GenreId = 1},
     new Books(){BookId = 2,Isbn =" 222",Title="The Young" ,Author = "Rabindra",Publication = "yy",PublishedDate = new DateOnly(2024, 11, 11),Edition = "second",Language = "Hindi",Description ="Journey of Human",Cost =200.99M,AvailableQuantity = 55,TotalQuantity = 100,GenreId = 1},
     new Books(){BookId = 3,Isbn =" 222",Title="The Man" ,Author = "Rabindra",Publication = "yy",PublishedDate = new DateOnly(2024, 11, 11),Edition = "second",Language = "Hindi",Description ="Journey of Human",Cost =200.99M,AvailableQuantity = 55,TotalQuantity = 100,GenreId = 1},

        };
        private List<Users> _users = new List<Users>
        {
new Users(){UserId =1,Name ="Sejal",Email = "ss@gmail.com",Phone = "456655",Address ="Bombay",PasswordHash = "tttt",Role =Data.Enums.User_Role.Consumer,AccountCreatedDate = new DateOnly(2024,10,10)},
new Users(){UserId =2,Name ="Kinjal",Email = "ss@gmail.com",Phone = "456655",Address ="Bombay",PasswordHash = "tttt",Role =Data.Enums.User_Role.Consumer,AccountCreatedDate = new DateOnly(2024,10,10)},
new Users(){UserId =3,Name ="Tushar",Email = "ss@gmail.com",Phone = "456655",Address ="Bombay",PasswordHash = "tttt",Role =Data.Enums.User_Role.Consumer,AccountCreatedDate = new DateOnly(2024,10,10)},
        };

        public  async Task<List<Books>> GetAllBooks()
        {
            return _books;

        }

        public  async Task<string> ValidateUser(string username, string password)
        {
            var exists = _users.Any(u => u.Name == username && u.PasswordHash == password);


            return exists ? "Success" : "Invalid username or password";
        }

        private readonly List<Users> _usersList = new List<Users>()
        {
            new Users() {UserId=1, Name="Niranjan", Email="niranjan@gmail.com", Phone="3462462466", Address="Mumbai", PasswordHash="niranjan@pass", Role=User_Role.Consumer, AccountCreatedDate=DateOnly.FromDateTime(DateTime.Now)}
        };

        public async Task<Users> CreateUser(Users user)
        {
            await Task.Delay(100);
            user.Role = User_Role.Consumer;
            user.AccountCreatedDate = DateOnly.FromDateTime(DateTime.Now);
            _usersList.Add(user);
            // _context.Add(user);
            // await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> UpdateUser(Users user)
        {
            await Task.Delay(100);
            Users existingUser = _usersList.FirstOrDefault(u => u.UserId == user.UserId);

            existingUser.Name = user.Name;
            existingUser.Phone = user.Phone;
            existingUser.Address = user.Address;
            // _context.Update(user);
            // await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IActionResult> DeleteUser(int userId)
        {
            await Task.Delay(100);
            Users user = _usersList.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                _usersList.Remove(user);
                //var user = await _context.Users.FindAsync(id);
                //if (user != null)
                //{
                //    _context.Users.Remove(user);
                //}

                //await _context.SaveChangesAsync();
                return new JsonResult("User deleted successfully!");
            }
            return new JsonResult("User not found!");
        }
        /* Logic after we connect to database
         * private BookHubDBContext _bookHubDBContext;
         * 
         * public CommonRepository(BookHubDBContext bookHubDBContext){
         * _bookHubDBContext = bookHubDBContext;
         * }
         * 
         * 
         
       
         public  async Task<List<Books>> GetAllBooks()
        {
            return await __bookHubDBContext.Books,ToList();

        }

        //Method to verify if the user is valid or not

        public async Task<Users> ValidateUser(string username, string password){
        //fetching user from database
          var user = await  _context.Users.
                                Where(u=>u.Name = username)
                                 .FirstOrDefaultAsync();

        if(user == null) {
        return null;
        }
        //if user is present then check the password

        if(user.PasswordHash == password){
        return user;}
       return null;
        }


        */

    }
}
