using Microsoft.EntityFrameworkCore;
using NewsAPI_Implementation.Models;

namespace NewsAPI_Implementation.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
