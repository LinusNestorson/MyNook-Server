using System;

namespace MyNook_Server.Api.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public decimal Amount { get; set; }
        
        public Guid UserId { get; set; }
    }
}
