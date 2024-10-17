using Booking_App_API.Models;

namespace Booking_App_API.Contracts.Users
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
