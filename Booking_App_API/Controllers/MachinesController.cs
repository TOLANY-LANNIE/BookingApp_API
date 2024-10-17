using Microsoft.AspNetCore.Mvc;
using Booking_App_API.Models;
using Supabase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking_App_API.Contracts.Machines;

namespace Booking_App_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachinesController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;

        public MachinesController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        // GET: api/machines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachinesResponse>>> GetAllMachines()
        {
            var response = await _supabaseClient.From<Machine>().Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound("No machines found.");
            }

            var machineResponses = response.Models.Select(m => new MachinesResponse
            {
                Id = m.Id,
                Name = m.Name,
                OperatorID = m.OperatorID
            }).ToList();

            return Ok(machineResponses);
        }

        // GET: api/machines/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MachinesResponse>> GetMachineById(string id)
        {
            var response = await _supabaseClient.From<Machine>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Machine with ID {id} not found.");
            }

            var machine = response.Models.First();
            var machineResponse = new MachinesResponse
            {
                Id = machine.Id,
                Name = machine.Name,
                OperatorID = machine.OperatorID
            };

            return Ok(machineResponse);
        }

        // POST: api/machines
        [HttpPost]
        public async Task<ActionResult<MachinesResponse>> CreateMachine([FromBody] MachinesRequest machineRequest)
        {
            var newMachine = new Machine
            {
                Name = machineRequest.Name,
                OperatorID = machineRequest.OperatorID
            };

            var response = await _supabaseClient.From<Machine>().Insert(newMachine);

            if (response.Models == null || !response.Models.Any())
            {
                return StatusCode(500, "Failed to create machine.");
            }

            var createdMachine = response.Models.First();
            var machineResponse = new MachinesResponse
            {
                Id = createdMachine.Id,
                Name = createdMachine.Name,
                OperatorID = createdMachine.OperatorID
            };

            return CreatedAtAction(nameof(GetMachineById), new { id = machineResponse.Id }, machineResponse);
        }

        // PUT: api/machines/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(string id, [FromBody] MachinesRequest machineRequest)
        {
            var response = await _supabaseClient.From<Machine>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Machine with ID {id} not found.");
            }

            var machineToUpdate = response.Models.First();

            machineToUpdate.Name = machineRequest.Name;
            machineToUpdate.OperatorID = machineRequest.OperatorID;

            var updateResponse = await _supabaseClient.From<Machine>().Update(machineToUpdate);

            if (updateResponse.Models == null || !updateResponse.Models.Any())
            {
                return StatusCode(500, "Failed to update machine.");
            }

            return NoContent(); // Success
        }

        // DELETE: api/machines/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(string id)
        {
            var response = await _supabaseClient.From<Machine>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"Machine with ID {id} not found.");
            }

            var machineToDelete = response.Models.First();

            var deleteResponse = await _supabaseClient.From<Machine>().Delete(machineToDelete);

            if (deleteResponse == null || deleteResponse.Models == null)
            {
                return StatusCode(500, "Failed to delete machine.");
            }

            return NoContent(); // Success
        }
    }
}
