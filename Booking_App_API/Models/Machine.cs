using Postgrest.Models;
using Postgrest.Attributes;

namespace Booking_App_API.Models
{
    [Table("machines")]
    public class Machine:BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("operator_id")]
        public string OperatorID { get; set; }



    }
}
