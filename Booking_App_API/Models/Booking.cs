using Postgrest.Models;
using Postgrest.Attributes;

namespace Booking_App_API.Models
{
    [Table("bookings")]
    public class Booking:BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("customer_id")]
        public string CustomerID { get; set; }

        [Column("machine_id")]
        public string MachineID { get; set; }

        [Column("booking_date")]
        public DateTime BookingDate { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}
