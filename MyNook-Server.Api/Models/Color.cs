using System;

namespace MyNook_Server.Api.Models
{
    public class Color
    {
        public Guid Id { get; set; }
        public required string Ncs { get; set; }
        public required string Name { get; set; }
        
        public Guid? RoomId { get; set; } // Optional link to a room
        public Guid UserId { get; set; }
    }
}
