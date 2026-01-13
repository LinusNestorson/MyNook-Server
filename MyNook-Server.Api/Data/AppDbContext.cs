using Microsoft.EntityFrameworkCore;
using MyNook_Server.Api.Models;

namespace MyNook_Server.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
