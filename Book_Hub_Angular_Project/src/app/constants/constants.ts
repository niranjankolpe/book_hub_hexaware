enum Fine_Type
{
    "Book Overdue" = 0,
    "Book Damage" = 1,
    "Book Lost" = 2
}

enum Fine_Paid_Status
{
    Unpaid,
    Paid
}

enum User_Role
{
    Consumer,
    Administrator
}

enum Notification_Type
{
    "Account Related",
    "Borrowed Book Related",
    "Reservation Book Related",
    "Other"
}

enum Action_Type
{
    "Logged In",
    "Logged Out",
    "Updated Account"
}

enum Reservation_Status
{
    Pending,
    Fullfilled,
    Cancelled,
    Expired
}

enum Contact_Us_Query_Type
{
    Complaint,
    Feeback,
    Suggestion
}

enum Contact_Us_Query_Status
{
    Pending,
    Acknowledged
}

enum Borrow_Status
{
    Borrowed,
    Returned,
    Lost
}

export const Constant = {
    BASE_URI : 'https://localhost:7251/api/',
    LOGIN:'Home/Login',
    Get_All_Genre:'Admin/GetGenre',
    Get_Logs:'Admin/GetLog',
    Get_Borrowed:'Admin/GetBorrowed',
    Get_Users:'Admin/GetUser',
    Get_Fines:'Admin/GetFines',
    Get_Notifications:'Admin/GetNotification',
    Get_Reservations:'Admin/GetReservation',
    Get_Books:'Home/GetAllBooks',
    Fine_Type: Fine_Type,
    Fine_Paid_Status: Fine_Paid_Status,
    User_Role: User_Role,
    Notification_Type: Notification_Type,
    Action_Type: Action_Type,
    Reservation_Status: Reservation_Status,
    Contact_Us_Query_Type: Contact_Us_Query_Type,
    Contact_Us_Query_Status: Contact_Us_Query_Status,
    Borrow_Status: Borrow_Status
}

