using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNook_Server.Api.Data;
using MyNook_Server.Api.Models;
using System.Security.Claims;

namespace MyNook_Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomManagerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomManagerController(AppDbContext context)
        {
            _context = context;
        }

        private Guid GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id != null ? Guid.Parse(id) : Guid.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var userId = GetUserId();
            return await _context.Rooms.Where(r => r.UserId == userId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            room.UserId = GetUserId();
            // Ensure ID is set if not provided (though Client usually doesn't, we can overwrite or new Guid)
            if (room.Id == Guid.Empty) room.Id = Guid.NewGuid();

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            var userId = GetUserId();
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (room == null) return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
