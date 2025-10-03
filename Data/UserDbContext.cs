using AuthDotNetApi.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthDotNetApi.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}