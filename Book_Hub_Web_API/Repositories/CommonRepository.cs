using Book_Hub_Web_API.Data;
using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Hub_Web_API.Repositories
{
    public class CommonRepository : ICommonRepository
    {

        private BookHubDBContext _bookHubDBContext;
        


        public CommonRepository(BookHubDBContext bookHubDBContext)
        {
            _bookHubDBContext = bookHubDBContext;
        }



        public async Task<List<Books>>GetAllBooks()
        {
            var books = await _bookHubDBContext.Books.ToListAsync();
            return books;
        }



        public async Task<Users> CreateUser(Users user)
        {
            await Task.Delay(100);
            try
            {
                user.Role = User_Role.Consumer;
                user.AccountCreatedDate = DateOnly.FromDateTime(DateTime.Now);
                await _bookHubDBContext.Users.AddAsync(user);

                await _bookHubDBContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: {ex.Message}", ex);
            }
        }



        public async Task<IActionResult> DeleteUser(int userId)
        {
            Users user = await _bookHubDBContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
               _bookHubDBContext.Users.Remove(user);
                await _bookHubDBContext.SaveChangesAsync();
                
                return new JsonResult("User deleted successfully!");
            }
            return new JsonResult("User not found!");
        }



        public  async Task<string> ValidateUser(string username, string password)
        {
            Users u =  _bookHubDBContext.Users.FirstOrDefault(u => u.Email == username && u.PasswordHash==password);
            if (u == null)
            {
                return "User does not exist";
            }
            return "User is valid";
        }



        public async Task<Users> UpdateUser(int userId, string name, string phone, string address)
        {
            Users existingUser = _bookHubDBContext.Users.FirstOrDefault(u => u.UserId == userId);

            if (existingUser == null)
            {
                return new Users() { Name = name };
            }

            if (existingUser != null)
            {
                existingUser.Name = name;
                existingUser.Phone = phone;
                existingUser.Address = address;
            }
            _bookHubDBContext.Update(existingUser);
            await _bookHubDBContext.SaveChangesAsync();


            return existingUser;
        }
    }
}
