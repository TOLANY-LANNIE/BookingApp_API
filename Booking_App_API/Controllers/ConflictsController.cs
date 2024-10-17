using Microsoft.AspNetCore.Mvc;
using Booking_App_API.Models;
using Supabase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking_App_API.Contracts.Conflicts;

namespace Booking_App_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConflictsController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;

        public ConflictsController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        // GET: api/conflicts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConflictResponse>>> GetAllConflicts()
        {
            var response = await _supabaseClient.From<Conflict>().Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound("No conflicts found.");
            }

            var conflictResponses = response.Models.Select(c => new ConflictResponse
            {
                Id = c.Id,
                BookingID = c.BookingID,
                ResolvedBy = c.ResolvedBy,
                OldMachineID = c.OldMachineID,
                NewMachineID = c.NewMachineID,
                ResolutionDate = c.ResolutionDate
            }).ToList();

            return Ok(conflictResponses);
        }

        // GET: api/conflicts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ConflictResponse>> GetConflictById(string id)
        {
            var response = await _supabaseClient.From<Conflict>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Conflict with ID {id} not found.");
            }

            var conflict = response.Models.First();
            var conflictResponse = new ConflictResponse
            {
                Id = conflict.Id,
                BookingID = conflict.BookingID,
                ResolvedBy = conflict.ResolvedBy,
                OldMachineID = conflict.OldMachineID,
                NewMachineID = conflict.NewMachineID,
                ResolutionDate = conflict.ResolutionDate
            };

            return Ok(conflictResponse);
        }

        // POST: api/conflicts
        [HttpPost]
        public async Task<ActionResult<ConflictResponse>> CreateConflict([FromBody] ConflictResponse conflictRequest)
        {
            var newConflict = new Conflict
            {
                BookingID = conflictRequest.BookingID,
                ResolvedBy = conflictRequest.ResolvedBy,
                OldMachineID = conflictRequest.OldMachineID,
                NewMachineID = conflictRequest.NewMachineID,
                ResolutionDate = conflictRequest.ResolutionDate
            };

            var response = await _supabaseClient.From<Conflict>().Insert(newConflict);

            if (response.Models == null || !response.Models.Any())
            {
                return StatusCode(500, "Failed to create conflict.");
            }

            var createdConflict = response.Models.First();
            var conflictResponse = new ConflictResponse
            {
                Id = createdConflict.Id,
                BookingID = createdConflict.BookingID,
                ResolvedBy = createdConflict.ResolvedBy,
                OldMachineID = createdConflict.OldMachineID,
                NewMachineID = createdConflict.NewMachineID,
                ResolutionDate = createdConflict.ResolutionDate
            };

            return CreatedAtAction(nameof(GetConflictById), new { id = conflictResponse.Id }, conflictResponse);
        }

        // PUT: api/conflicts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConflict(string id, [FromBody] ConflictResponse conflictRequest)
        {
            // Retrieve the existing conflict
            var response = await _supabaseClient.From<Conflict>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Conflict with ID {id} not found.");
            }

            var conflictToUpdate = response.Models.First();

            // Update the conflict details
            conflictToUpdate.BookingID = conflictRequest.BookingID;
            conflictToUpdate.ResolvedBy = conflictRequest.ResolvedBy;
            conflictToUpdate.OldMachineID = conflictRequest.OldMachineID;
            conflictToUpdate.NewMachineID = conflictRequest.NewMachineID;
            conflictToUpdate.ResolutionDate = conflictRequest.ResolutionDate;

            var updateResponse = await _supabaseClient.From<Conflict>().Update(conflictToUpdate);

            if (updateResponse.Models == null || !updateResponse.Models.Any())
            {
                return StatusCode(500, "Failed to update conflict.");
            }

            return NoContent(); // Successfully updated the conflict
        }

        // DELETE: api/conflicts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConflict(string id)
        {
            // Retrieve the conflict with the provided ID
            var response = await _supabaseClient.From<Conflict>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Conflict with ID {id} not found.");
            }

            var conflictToDelete = response.Models.First();

            // Attempt to delete the conflict
            var deleteResponse = await _supabaseClient.From<Conflict>().Delete(conflictToDelete);

            if (deleteResponse.Models == null || !deleteResponse.Models.Any())
            {
                return StatusCode(500, "Failed to delete conflict.");
            }

            return NoContent(); // Successfully deleted the conflict
        }
    }
}
