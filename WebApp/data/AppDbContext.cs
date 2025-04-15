using Microsoft.EntityFrameworkCore;
using WebApp.Components;

namespace WebApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    // Database table model property which enables to edit for record in DB
    public DbSet<Product> Products { get; set; } = default!;
} // end of AppDbContext