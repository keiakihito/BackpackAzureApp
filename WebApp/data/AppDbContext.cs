using Microsoft.EntityFrameworkCore;
using WebApp.Components;

namespace WebApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; } = default!;
} // end of AppDbContext