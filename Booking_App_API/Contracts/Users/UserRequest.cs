using Booking_App_API.Models;

namespace Booking_App_API.Contracts.Users
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
