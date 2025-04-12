using Xunit;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace WebApp.Tests
{
    public class ProductEditTests
    {
        private DbContextOptions<AppDbContext> GetOptions() =>
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"EditTestDb_{Guid.NewGuid()}")
                .Options;

        [Fact]
        public async Task EditProduct_ShouldUpdateFields()
        {
            // Arrange
            var options = GetOptions();

            using (var context = new AppDbContext(options))
            {
                var product = new Product
                {
                    brand = "EditMe",
                    price = 100,
                    weight_capacity_kg = 10,
                    waterproof = false,
                    size = "M",
                    color = "Blue",
                    style = "Classic",
                    compartments = 2
                };

                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new AppDbContext(options))
            {
                var product = await context.Products.FirstAsync(p => p.brand == "EditMe");

                product.price = 200;
                product.color = "Red";
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new AppDbContext(options))
            {
                var updated = await context.Products.FirstAsync(p => p.brand == "EditMe");
                Assert.Equal(200, updated.price);
                Assert.Equal("Red", updated.color);
            }
        }
    }
}