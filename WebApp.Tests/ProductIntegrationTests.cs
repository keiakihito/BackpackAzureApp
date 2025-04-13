using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using Xunit;

public class ProductIntegrationTests
{
    [Fact]
    public async Task CreateAndRetrieveProduct_ShouldWorkCorrectly()
    {
        // Create virtual DB on memory
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ProductDb")
            .Options;
        
        // Create record and save it 
        using (var context = new AppDbContext(options))
        {
            var product = new Product { brand = "Test Brand", price = 12.5M };
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }
        
        // Check created record matches initial value
        using (var context = new AppDbContext(options))
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.brand == "Test Brand");
            Assert.NotNull(product);
            Assert.Equal(12.5M, product.price);
        }
    }
}