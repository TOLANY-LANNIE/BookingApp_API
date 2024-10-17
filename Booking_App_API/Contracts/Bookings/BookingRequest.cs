﻿using Booking_App_API.Models;

namespace Booking_App_API.Contracts.Bookings
{
    public class BookingRequest
    {
        public string CustomerID { get; set; }
        public string MachineID { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
    }
}
