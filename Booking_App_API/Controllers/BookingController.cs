using Booking_App_API.Models;
using Booking_App_API.Contracts.Bookings;
using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking_App_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;

        public BookingsController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAllBookings()
        {
            var response = await _supabaseClient.From<Booking>().Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound("No bookings found.");
            }

            var bookingResponses = response.Models.Select(b => new BookingResponse
            {
                Id = b.Id,
                CustomerID = b.CustomerID,
                MachineID = b.MachineID,
                BookingDate = b.BookingDate,
                Status = b.Status
            }).ToList();

            return Ok(bookingResponses);
        }

        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingResponse>> GetBookingById(string id)
        {
            var response = await _supabaseClient.From<Booking>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            var booking = response.Models.First();
            var bookingResponse = new BookingResponse
            {
                Id = booking.Id,
                CustomerID = booking.CustomerID,
                MachineID = booking.MachineID,
                BookingDate = booking.BookingDate,
                Status = booking.Status
            };

            return Ok(bookingResponse);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<ActionResult<BookingResponse>> CreateBooking([FromBody] BookingRequest bookingRequest)
        {
            var newBooking = new Booking
            {
                CustomerID = bookingRequest.CustomerID,
                MachineID = bookingRequest.MachineID,
                BookingDate = bookingRequest.BookingDate,
                Status = bookingRequest.Status
            };

            var response = await _supabaseClient.From<Booking>().Insert(newBooking);

            if (response.Models == null || !response.Models.Any())
            {
                return StatusCode(500, "Failed to create booking.");
            }

            var createdBooking = response.Models.First();
            var bookingResponse = new BookingResponse
            {
                Id = createdBooking.Id,
                CustomerID = createdBooking.CustomerID,
                MachineID = createdBooking.MachineID,
                BookingDate = createdBooking.BookingDate,
                Status = createdBooking.Status
            };

            return CreatedAtAction(nameof(GetBookingById), new { id = bookingResponse.Id }, bookingResponse);
        }

        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] BookingRequest bookingRequest)
        {
            var response = await _supabaseClient.From<Booking>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            var bookingToUpdate = response.Models.First();
            bookingToUpdate.CustomerID = bookingRequest.CustomerID;
            bookingToUpdate.MachineID = bookingRequest.MachineID;
            bookingToUpdate.BookingDate = bookingRequest.BookingDate;
            bookingToUpdate.Status = bookingRequest.Status;

            var updateResponse = await _supabaseClient.From<Booking>().Update(bookingToUpdate);

            if (updateResponse.Models == null || !updateResponse.Models.Any())
            {
                return StatusCode(500, "Failed to update booking.");
            }

            return NoContent();
        }

        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            // Retrieve the booking with the provided ID
            var response = await _supabaseClient.From<Booking>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            // If no booking is found with the provided ID
            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Booking with ID {id} not found.");
            }

            var bookingToDelete = response.Models.First();

            // Attempt to delete the booking
            var deleteResponse = await _supabaseClient.From<Booking>().Delete(bookingToDelete);

            // Check if the deleteResponse contains the deleted booking or if the operation was successful
            if (deleteResponse.Models == null || !deleteResponse.Models.Any())
            {
                return StatusCode(500, "Failed to delete booking.");
            }

            return NoContent(); // Successfully deleted the booking
        }
    }
}
