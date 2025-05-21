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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] User updated)
        {
            if (id != updated.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = updated.Name;
            user.Email = updated.Email;
            user.Age = updated.Age;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
