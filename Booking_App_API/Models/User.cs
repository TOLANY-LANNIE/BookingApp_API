using Postgrest.Models;
using Postgrest.Attributes;

namespace Booking_App_API.Models
{
    [Table("users")]
    public class User: BaseModel
    {
        [PrimaryKey("id",false)]
        public string Id { get; set; }

        [Column("fullname")]
        public string Fullname { get; set; }
       
        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role_id")]
        public string Role { get; set; }
    }
}
