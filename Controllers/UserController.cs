// Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JamilDotnetMicrosoftCertificate.Data;
using JamilDotnetMicrosoftCertificate.Models;
using System;
using System.Threading.Tasks;

namespace JamilDotnetMicrosoftCertificate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public UsersController(ApplicationDbContext db) => _db = db;

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Users.ToListAsync());

        // GET: api/users/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User updated)
        {
            if (id != updated.Id) 
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var existing = await _db.Users.FindAsync(id);
            if (existing is null) 
                return NotFound();

            existing.Name  = updated.Name;
            existing.Email = updated.Email;
            existing.Age   = updated.Age;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null) 
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
