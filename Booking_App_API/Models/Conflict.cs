using Postgrest.Models;
using Postgrest.Attributes;

namespace Booking_App_API.Models
{
    [Table("conflicts")]
    public class Conflict:BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; }

        [Column("booking_id")]
        public string BookingID { get; set; }

        [Column("resolved_by")]
        public string ResolvedBy { get; set; }

        [Column("old_machine_id")]
        public string OldMachineID { get; set; }

        [Column("new_machine_id")]
        public string NewMachineID { get; set; }

        [Column("resolution_date")]
        public DateTime ResolutionDate { get; set; }
    }
}
