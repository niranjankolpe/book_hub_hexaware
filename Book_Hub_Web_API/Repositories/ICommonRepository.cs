using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Book_Hub_Web_API.Repositories
{
    public interface ICommonRepository
    {
        Task<Users> ValidateUser(Validate_User_DTO validate_User_DTO);

        Task Login(int userId);

        Task Logout(int userId);

        Task<List<Books>> GetAllBooks();

        Task<Users> CreateUser(Create_User_DTO create_User_DTO);

        Task<Users> UpdateUser(UpdateUser_DTO updateUser_DTO);

        Task<string> DeleteUser(int userId);

        Task<string> ForgotPassword(string emailAddress, string newPassword);

    }
}
