using System;

namespace MyNook_Server.Api.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Function { get; set; } = "Bedroom";
        public string Floor { get; set; } = "1";
        public double? Area { get; set; }
        
        public Guid UserId { get; set; }
    }
}
