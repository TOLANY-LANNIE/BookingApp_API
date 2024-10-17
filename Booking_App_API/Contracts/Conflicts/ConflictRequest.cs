namespace Booking_App_API.Contracts.Conflicts
{
    public class ConflictRequest
    {
        public string BookingID { get; set; }
        public string ResolvedBy { get; set; }
        public string OldMachineID { get; set; } // Should be string as per your ID structure
        public string NewMachineID { get; set; } // Should be string as per your ID structure
        public DateTime ResolutionDate { get; set; } // Assuming string for flexibility; use DateTime if you prefer
    }
}
