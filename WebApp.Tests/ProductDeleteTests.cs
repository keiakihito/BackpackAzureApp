using Xunit;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace WebApp.Tests
{
    public class ProductDeleteTests
    {
        private DbContextOptions<AppDbContext> GetOptions() =>
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"DeleteTestDb_{Guid.NewGuid()}")
                .Options;

        [Fact]
        public async Task DeleteProduct_ShouldRemoveFromDatabase()
        {
            // Arrange
            var options = GetOptions();

            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product
                {
                    brand = "DeleteMe",
                    price = 123,
                    weight_capacity_kg = 10,
                    waterproof = false,
                    size = "S",
                    color = "Gray",
                    style = "Simple",
                    compartments = 1
                });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(options))
            {
                var product = await context.Products.FirstAsync(p => p.brand == "DeleteMe");
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new AppDbContext(options))
            {
                var deleted = await context.Products.FirstOrDefaultAsync(p => p.brand == "DeleteMe");
                Assert.Null(deleted);
            }
        }
    }
}