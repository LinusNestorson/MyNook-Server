using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNook_Server.Api.Data;
using System.Security.Claims;

namespace MyNook_Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        private Guid GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id != null ? Guid.Parse(id) : Guid.Empty;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var userId = GetUserId();
            var colorCount = await _context.Colors.CountAsync(c => c.UserId == userId);
            
            return Ok(new 
            { 
                totalColors = colorCount
                // Add more stats here
            });
        }
    }
}
