﻿using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Book_Hub_Web_API.Repositories
{
    public interface ICommonRepository
    {
        Task<string> ValidateUser(string username, string password);
         Task<List<Books>> GetAllBooks();

        Task<Users> CreateUser(Users user);

        Task<Users> UpdateUser(int userId, string name, string phone, string address);

        Task<IActionResult> DeleteUser(int userId);

    }
}
