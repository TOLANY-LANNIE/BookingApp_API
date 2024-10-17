using Postgrest.Models;
using Postgrest.Attributes;

namespace Booking_App_API.Models
{
    [Table("users")]
    public class User: BaseModel
    {
        [PrimaryKey("id",false)]
        public string Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public string Role { get; set; }
    }
}
