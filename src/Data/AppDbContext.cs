using GraphQLPoC.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLPoC.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
    : base(options)
    {
    }

    public DbSet<Platform> Platform { get; set; }
}