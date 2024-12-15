using Book_Hub_Web_API.Data;
using Book_Hub_Web_API.Data.DTO;
using Book_Hub_Web_API.Data.Enums;
using Book_Hub_Web_API.Models;
using Book_Hub_Web_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Hub_Web_API.Repositories
{
    public class CommonRepository : ICommonRepository
    {

        private BookHubDBContext _bookHubDBContext;

        //private INotificationService _notificationService;

        public CommonRepository(BookHubDBContext bookHubDBContext)
        {
            _bookHubDBContext = bookHubDBContext;
        }

        public async Task<List<Books>>GetAllBooks()
        {
            var books = await _bookHubDBContext.Books.ToListAsync();
            if (books == null)
            {
                throw new Exception("No book found!");
            }
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

                Notifications notification = new Notifications()
                {
                    UserId = u.UserId,
                    MessageType = Notification_Type.Account_Related,
                    MessageDescription = $"Welcome to Book Hub, Dear {u.Name}",
                    SentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                await _bookHubDBContext.Notifications.AddAsync(notification);
                await _bookHubDBContext.SaveChangesAsync();

                // Create the log entry
                LogUserActivity logUserActivity = new LogUserActivity
                {
                    UserId = u.UserId,
                    ActionType = Action_Type.UpdatedAccount,
                    Timestamp = DateTime.Now
                };

                // Add the log entry and save changes
                await _bookHubDBContext.LogUserActivity.AddAsync(logUserActivity);
                await _bookHubDBContext.SaveChangesAsync(); // Save the log entry

                return u;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: {ex.Message}", ex);
            }
        }



        public async Task<Users> DeleteUser(int userId)
        {
            Users ? user = _bookHubDBContext.Users.First(u => u.UserId == userId);
            if (user != null)
            {
                List<Borrowed> borrows = _bookHubDBContext.Borrowed.Where(b => b.UserId == userId && b.BorrowStatus == Borrow_Status.Borrowed).ToList();
                List<Reservations> reservations = _bookHubDBContext.Reservations.Where(b => b.UserId == userId && b.ReservationStatus == Reservation_Status.Pending).ToList();

                if (borrows.Count > 0)
                {
                    throw new Exception("You have some borrowed books. Return them before deleting your account!");
                }
                else if (reservations.Count > 0)
                {
                    throw new Exception("You have some pending reservations. Cancel them before deleting your account!");
                }

                Users temporayUser = new Users()
                {
                    Name = user.Name,
                    Email = user.Email
                };

                Notifications notification = new Notifications()
                {
                    UserId = user.UserId,
                    MessageType = Notification_Type.Account_Related,
                    MessageDescription = $"\nDear {user.Name},\n\nSuccessfully Deleted your Account details at Book Hub! Sorry to see you go :(\n\nRegards,\nBook Hub",
                    SentDate = DateOnly.FromDateTime(DateTime.Now)
                };
                await _bookHubDBContext.Notifications.AddAsync(notification);
                await _bookHubDBContext.SaveChangesAsync();

                // Create the log entry
                LogUserActivity logUserActivity = new LogUserActivity
                {
                    UserId = user.UserId,
                    ActionType = Action_Type.UpdatedAccount,
                    Timestamp = DateTime.Now
                };

                // Add the log entry and save changes
                await _bookHubDBContext.LogUserActivity.AddAsync(logUserActivity);
                await _bookHubDBContext.SaveChangesAsync(); // Save the log entry

                _bookHubDBContext.Users.Remove(user);
                await _bookHubDBContext.SaveChangesAsync();

                return temporayUser;
            }
            throw new Exception($"No user of User Id: {userId} found!");
        }

        

        public async Task<Users> ValidateUser(Validate_User_DTO validate_User_DTO)
        {
            Users? user = await _bookHubDBContext.Users.FirstOrDefaultAsync(u => u.Email == validate_User_DTO.Email && u.PasswordHash == validate_User_DTO.PasswordHash);
            if (user == null)
            {
                throw new Exception("User with those credentials not found!");
            }
            return user;
        }

        public async Task Login(int userId)
        {
            // Create the log entry
            LogUserActivity logUserActivity = new LogUserActivity
            {
                UserId = userId,
                ActionType = Action_Type.Login,
                Timestamp = DateTime.Now
            };

            // Add the log entry and save changes
            await _bookHubDBContext.LogUserActivity.AddAsync(logUserActivity);
            await _bookHubDBContext.SaveChangesAsync(); // Save the log entry
        }

        public async Task Logout(int userId)
        {
            // Create the log entry
            LogUserActivity logUserActivity = new LogUserActivity
            {
                UserId = userId,
                ActionType = Action_Type.Logout,
                Timestamp = DateTime.Now
            };

            // Add the log entry and save changes
            await _bookHubDBContext.LogUserActivity.AddAsync(logUserActivity);
            await _bookHubDBContext.SaveChangesAsync(); // Save the log entry
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
            if (updateUser_DTO.Name != null)
            {
                existingUser.Name = updateUser_DTO.Name;
            }
            if (updateUser_DTO.Phone != null)
            {
                existingUser.Phone = updateUser_DTO.Phone;
            }
            if (updateUser_DTO.Address != null)
            {
                existingUser.Address = updateUser_DTO.Address;
            }

            // Update the user in the database
            _bookHubDBContext.Update(existingUser);
            await _bookHubDBContext.SaveChangesAsync(); // Save the updated user details

            Notifications notification = new Notifications()
            {
                UserId = existingUser.UserId,
                MessageType = Notification_Type.Account_Related,
                MessageDescription = $"Dear {existingUser.Name},\n\nSuccessfully Updated your Account details at Book Hub!\n\nRegards,\nBook Hub",
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            await _bookHubDBContext.Notifications.AddAsync(notification);
            await _bookHubDBContext.SaveChangesAsync();

            // Create the log entry
            LogUserActivity logUserActivity = new LogUserActivity
            {
                UserId = updateUser_DTO.UserId,
                ActionType = Action_Type.UpdatedAccount,
                Timestamp = DateTime.Now
            };

            // Add the log entry and save changes
            await _bookHubDBContext.LogUserActivity.AddAsync(logUserActivity);
            await _bookHubDBContext.SaveChangesAsync(); // Save the log entry

            return existingUser;
        }

        public async Task<Users> ForgotPassword(string emailAddress, string newPassword)
        {
            Users existingUser = await _bookHubDBContext.Users.FirstOrDefaultAsync(u => u.Email == emailAddress);
            if (existingUser == null)
            {
                throw new Exception("User with this email not found");
            }
            existingUser.PasswordHash = newPassword;

            // Update the user in the database
            _bookHubDBContext.Update(existingUser);
            await _bookHubDBContext.SaveChangesAsync(); // Save the updated user details

            Notifications notification = new Notifications()
            {
                UserId = existingUser.UserId,
                MessageType = Notification_Type.Account_Related,
                MessageDescription = $"Dear {existingUser.Name},\n\nSuccessfully Updated your Account Password at Book Hub!\n\nRegards,\nBook Hub",
                SentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            await _bookHubDBContext.Notifications.AddAsync(notification);
            await _bookHubDBContext.SaveChangesAsync();

            // Create the log entry
            LogUserActivity logUserActivity = new LogUserActivity
            {
                UserId = existingUser.UserId,
                ActionType = Action_Type.UpdatedAccount,
                Timestamp = DateTime.Now
            };

            // Add the log entry and save changes
            await _bookHubDBContext.LogUserActivity.AddAsync(logUserActivity);
            await _bookHubDBContext.SaveChangesAsync(); // Save the log entry
            return existingUser;
        }

        public async Task<ContactUs> AddContactUsQuery(Contact_Us_DTO contact_Us_DTO)
        {
            ContactUs contactUs = new ContactUs()
            {
                Email = contact_Us_DTO.Email,
                Query_Type = contact_Us_DTO.Query_Type,
                Description = contact_Us_DTO.Description,
                QueryCreatedDate = DateOnly.FromDateTime(DateTime.Now),
                Query_Status = Contact_Us_Query_Status.Pending
            };
            await _bookHubDBContext.ContactUs.AddAsync(contactUs);
            await _bookHubDBContext.SaveChangesAsync();
            return contactUs;
        }
    }
}
