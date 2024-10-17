using Microsoft.AspNetCore.Mvc;
using Booking_App_API.Models;
using Supabase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking_App_API.Contracts.Users;

namespace Booking_App_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;

        public UsersController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var response = await _supabaseClient.From<User>().Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound("No users found.");
            }

            var userResponses = response.Models.Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            }).ToList();

            return Ok(userResponses);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(string id)
        {
            var response = await _supabaseClient.From<User>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var user = response.Models.First();
            var userResponse = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            return Ok(userResponse);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserRequest userRequest)
        {
            var newUser = new User
            {
                Username = userRequest.Username,
                Password = userRequest.Password, // Remember to hash this in a real application
                Role = userRequest.Role
            };

            var response = await _supabaseClient.From<User>().Insert(newUser);

            if (response.Models == null || !response.Models.Any())
            {
                return StatusCode(500, "Failed to create user.");
            }

            var createdUser = response.Models.First();
            var userResponse = new UserResponse
            {
                Id = createdUser.Id,
                Username = createdUser.Username,
                Role = createdUser.Role
            };

            return CreatedAtAction(nameof(GetUserById), new { id = userResponse.Id }, userResponse);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserRequest userRequest)
        {
            // Get the existing user
            var response = await _supabaseClient.From<User>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var userToUpdate = response.Models.First();

            // Update the user details
            userToUpdate.Username = userRequest.Username;
            userToUpdate.Password = userRequest.Password; // Ensure hashing in real applications
            userToUpdate.Role = userRequest.Role;

            var updateResponse = await _supabaseClient.From<User>().Update(userToUpdate);

            if (updateResponse.Models == null || !updateResponse.Models.Any())
            {
                return StatusCode(500, "Failed to update user.");
            }

            return NoContent(); // Success
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            // Retrieve the user with the provided ID
            var response = await _supabaseClient.From<User>().Filter("id", Postgrest.Constants.Operator.Equals, id).Get();

            // If no user is found with the provided ID
            if (response.Models == null || response.Models.Count == 0)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var userToDelete = response.Models.First();

            // Attempt to delete the user
            var deleteResponse = await _supabaseClient.From<User>().Delete(userToDelete);

            // Check if the deleteResponse contains the deleted user or if the operation was successful
            if (deleteResponse.Models == null || !deleteResponse.Models.Any())
            {
                return StatusCode(500, "Failed to delete user.");
            }

            return NoContent(); // Successfully deleted the user
        }
    }
}
