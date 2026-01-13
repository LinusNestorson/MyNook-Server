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
    public class ColorTrackerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColorTrackerController(AppDbContext context)
        {
            _context = context;
        }

        private Guid GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id != null ? Guid.Parse(id) : Guid.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors()
        {
            var userId = GetUserId();
            return await _context.Colors.Where(c => c.UserId == userId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            color.UserId = GetUserId();
            if (color.Id == Guid.Empty) color.Id = Guid.NewGuid();

            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColors), new { id = color.Id }, color);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(Guid id)
        {
            var userId = GetUserId();
            var color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (color == null) return NotFound();

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
