using Microsoft.AspNetCore.Mvc;
using Booking_App_API.Models;
using Supabase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking_App_API.Contracts.Roles;

namespace Booking_App_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public RolesController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // GET: api/roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetRoles()
        {
            var roles = await _supabase.From<Roles>().Get();

            var response = roles.Models.Select(r => new RoleResponse
            {
                Id = r.Id,
                Name = r.Name
            });

            return Ok(response);
        }

        // GET: api/roles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponse>> GetRoleById(string id)
        {
            var role = await _supabase.From<Roles>().Where(r => r.Id == id).Single();
            if (role == null)
            {
                return NotFound();
            }

            var response = new RoleResponse
            {
                Id = role.Id,
                Name = role.Name
            };

            return Ok(response);
        }

        // POST: api/roles
        [HttpPost]
        public async Task<ActionResult<RoleResponse>> CreateRole([FromBody] RoleRequest request)
        {
            var newRole = new Roles
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name
            };

            var response = await _supabase.From<Roles>().Insert(newRole);

            var createdRole = response.Models.FirstOrDefault();
            if (createdRole == null)
            {
                return BadRequest("Failed to create role.");
            }

            var roleResponse = new RoleResponse
            {
                Id = createdRole.Id,
                Name = createdRole.Name
            };

            return CreatedAtAction(nameof(GetRoleById), new { id = roleResponse.Id }, roleResponse);
        }

        // PUT: api/roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleRequest request)
        {
            var role = await _supabase.From<Roles>().Where(r => r.Id == id).Single();
            if (role == null)
            {
                return NotFound();
            }

            role.Name = request.Name;
            await _supabase.From<Roles>().Update(role);

            return NoContent();
        }

        // DELETE: api/roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _supabase.From<Roles>().Where(r => r.Id == id).Single();
            if (role == null)
            {
                return NotFound();
            }

            await _supabase.From<Roles>().Delete(role);
            return NoContent();
        }
    }
}
