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



        public async Task<Users> CreateUser(Create_User_DTO create_User_DTO)
        {
            await Task.Delay(100);
            try
            {
                Users u = new Users()
                {
                    Name = create_User_DTO.Name,
                    Email = create_User_DTO.Email,
                    Phone = create_User_DTO.Phone,
                    Address = create_User_DTO.Address,
                    PasswordHash = create_User_DTO.PasswordHash
                };
                u.Role = User_Role.Consumer;
                u.AccountCreatedDate = DateOnly.FromDateTime(DateTime.Now);
                await _bookHubDBContext.Users.AddAsync(u);

                await _bookHubDBContext.SaveChangesAsync();

                return u;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: {ex.Message}", ex);
            }
        }



        public async Task<string> DeleteUser(int userId)
        {
            Users ? user = await _bookHubDBContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
               _bookHubDBContext.Users.Remove(user);
                await _bookHubDBContext.SaveChangesAsync();
                
                return "User deleted successfully!";
            }
            return "User not found!";
        }



        public  async Task<string> ValidateUser(Validate_User_DTO validate_User_DTO)
        {
            Users? u =  await _bookHubDBContext.Users.FirstOrDefaultAsync(u => u.Email == validate_User_DTO.Email && u.PasswordHash == validate_User_DTO.PasswordHash);
            if (u == null)
            {
                return "User does not exist";
            }
            return "User is valid";
        }



        public async Task<Users> UpdateUser(UpdateUser_DTO updateUser_DTO)
        {
            // Retrieve the existing user
            Users? existingUser = await _bookHubDBContext.Users.FirstOrDefaultAsync(u => u.UserId == updateUser_DTO.UserId);

            // If user does not exist, return a placeholder user object
            if (existingUser == null)
            {
                return new Users { Name = updateUser_DTO.Name };
            }

            // Update the user's details
            existingUser.Name = updateUser_DTO.Name;
            existingUser.Phone = updateUser_DTO.Phone;
            existingUser.Address = updateUser_DTO.Address;

            // Update the user in the database
            _bookHubDBContext.Update(existingUser);
            await _bookHubDBContext.SaveChangesAsync(); // Save the updated user details

            // Create the log entry
            LogUserActivity logUserActivity = new LogUserActivity
            {
                UserId = updateUser_DTO.UserId,
                ActionType = Action_Type.UpdatedAccount,
                Timestamp = DateTime.UtcNow
            };

            // Add the log entry and save changes
            await _bookHubDBContext.LogUserActivity.AddAsync(logUserActivity);
            await _bookHubDBContext.SaveChangesAsync(); // Save the log entry

            return existingUser;
        }
    }
}
