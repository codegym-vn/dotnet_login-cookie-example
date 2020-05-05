using Microsoft.EntityFrameworkCore;
using CookieTest.Models;

namespace CookieTest.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}