namespace Book_Hub_Web_API.Data.DTO
{
    public class Reset_Password_DTO
    {
        public int UserId { get; set; }
        public string? Email { get; set; }

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }
    }
}
