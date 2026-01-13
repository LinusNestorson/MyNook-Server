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
    public class BankImportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BankImportController(AppDbContext context)
        {
            _context = context;
        }

        private Guid GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return id != null ? Guid.Parse(id) : Guid.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var userId = GetUserId();
            return await _context.Transactions.Where(t => t.UserId == userId).OrderByDescending(t => t.Date).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            transaction.UserId = GetUserId();
            if (transaction.Id == Guid.Empty) transaction.Id = Guid.NewGuid();

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransactions), new { id = transaction.Id }, transaction);
        }

        // Bulk import endpoint (placeholder logic)
        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions([FromBody] List<Transaction> transactions)
        {
            var userId = GetUserId();
            foreach (var t in transactions)
            {
                t.UserId = userId;
                if (t.Id == Guid.Empty) t.Id = Guid.NewGuid();
                _context.Transactions.Add(t);
            }
            await _context.SaveChangesAsync();
            return Ok(new { count = transactions.Count });
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, Transaction transaction)
        {
             if (id != transaction.Id) return BadRequest();
             
             var userId = GetUserId();
             var existing = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
             if (existing == null) return NotFound();
             
             existing.Category = transaction.Category;
             // Update other fields as needed
             
             await _context.SaveChangesAsync();
             return NoContent();
        }
    }
}
