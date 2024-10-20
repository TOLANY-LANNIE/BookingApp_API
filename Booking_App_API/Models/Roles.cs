using Postgrest.Models;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Booking_App_API.Models
{
    [Table("roles")]
    public class Roles: BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("role_name")]
        public string Name { get; set; }
    }
}
