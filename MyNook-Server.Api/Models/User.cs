using System;

namespace MyNook_Server.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
    }
}
