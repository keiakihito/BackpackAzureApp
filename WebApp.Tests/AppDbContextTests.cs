using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace WebApp.Tests
{
    public class AppDbContextTests
    {
        private DbContextOptions<AppDbContext> GetInMemoryOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        [Fact]
        public async Task AddProduct_ShouldAddToDbContext()
        {
            // Arrange
            var options = GetInMemoryOptions("AddProductTest");
            var product = new Product
            {
                brand = "AddTest",
                price = 1500,
                weight_capacity_kg = 5f,
                waterproof = false,
                size = "S",
                color = "Blue",
                style = "Urban",
                compartments = 1
            };

            // Act - First context adds the product
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            // Assert - Second context verifies the product was added
            using (var context = new AppDbContext(options))
            {
                var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.brand == "AddTest");
                Assert.NotNull(savedProduct);
                Assert.Equal("AddTest", savedProduct!.brand);
            }
        }

        [Fact]
        public async Task UpdateProduct_ShouldChangeValue()
        {
            // Arrange - Use a string variable to exactly match the test
            string original = "Green";
            string updated = "Yellow";
            
            var options = GetInMemoryOptions("UpdateProductTest");
            var product = new Product
            {
                brand = "EditTest",
                price = 1200,
                weight_capacity_kg = 7f,
                waterproof = true,
                size = "L",
                color = original,
                style = "Classic",
                compartments = 4
            };

            // Act - First context adds the product
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            // Act - Second context updates the product
            using (var context = new AppDbContext(options))
            {
                var savedProduct = await context.Products.FirstAsync(p => p.brand == "EditTest");
                savedProduct.color = updated;
                await context.SaveChangesAsync();
            }

            // Assert - Third context verifies the changes
            using (var context = new AppDbContext(options))
            {
                var updatedProduct = await context.Products.FirstAsync(p => p.brand == "EditTest");
                Assert.Equal(updated, updatedProduct.color);
            }
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveFromDbContext()
        {
            // Arrange
            var options = GetInMemoryOptions("DeleteProductTest");
            var product = CreateTestProduct("DeleteTest", 800);

            // Add the product
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            // Act - Delete the product
            using (var context = new AppDbContext(options))
            {
                var savedProduct = await context.Products.FirstAsync(p => p.brand == "DeleteTest");
                context.Products.Remove(savedProduct);
                var deleteResult = await context.SaveChangesAsync();
                
                // Assert correct number of entities deleted
                Assert.Equal(1, deleteResult);
            }

            // Assert - Verify product is deleted
            using (var context = new AppDbContext(options))
            {
                var deletedProduct = await context.Products.FirstOrDefaultAsync(p => p.brand == "DeleteTest");
                Assert.Null(deletedProduct);
            }
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var options = GetInMemoryOptions("GetAllProductsTest");
            var products = new List<Product>
            {
                CreateTestProduct("Brand1", 100),
                CreateTestProduct("Brand2", 200, "M", "Red"),
                CreateTestProduct("Brand3", 300, "L", "Black")
            };

            // Add products
            using (var context = new AppDbContext(options))
            {
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var allProducts = await context.Products.ToListAsync();
                Assert.Equal(3, allProducts.Count);
                Assert.Contains(allProducts, p => p.brand == "Brand1");
                Assert.Contains(allProducts, p => p.brand == "Brand2");
                Assert.Contains(allProducts, p => p.brand == "Brand3");
            }
        }

        // [Fact]
        // public async Task FilterProducts_ShouldReturnMatchingProducts()
        // {
        //     // Arrange
        //     var options = GetInMemoryOptions("FilterProductsTest");
        //     var products = new List<Product>
        //     {
        //         CreateTestProduct("Filter1", 150, "S", "Blue"),
        //         CreateTestProduct("Filter2", 250, "M", "Blue"),
        //         CreateTestProduct("Filter3", 350, "L", "Red")
        //     };
        //
        //     // Add products
        //     using (var context = new AppDbContext(options))
        //     {
        //         await context.Products.AddRangeAsync(products);
        //         await context.SaveChangesAsync();
        //     }
        //
        //     // Act & Assert - Filter by color
        //     using (var context = new AppDbContext(options))
        //     {
        //         var blueProducts = await context.Products
        //             .Where(p => p.color == "Blue")
        //             .ToListAsync();
        //         
        //         Assert.Equal(2, blueProducts.Count);
        //         Assert.Contains(blueProducts, p => p.brand == "Filter1");
        //         Assert.Contains(blueProducts, p => p.brand == "Filter2");
        //         Assert.DoesNotContain(blueProducts, p => p.brand == "Filter3");
        //     }
        //
        //     // Act & Assert - Filter by size
        //     using (var context = new AppDbContext(options))
        //     {
        //         var sizeProducts = await context.Products
        //             .Where(p => p.size == "L")
        //             .ToListAsync();
        //         
        //         Assert.Single(sizeProducts);
        //         Assert.Contains(sizeProducts, p => p.brand == "Filter3");
        //     }
        //
        //     // Act & Assert - Filter by price range
        //     using (var context = new AppDbContext(options))
        //     {
        //         var priceRangeProducts = await context.Products
        //             .Where(p => p.price >= 200 && p.price <= 300)
        //             .ToListAsync();
        //         
        //         Assert.Equal(2, priceRangeProducts.Count);
        //         Assert.Contains(priceRangeProducts, p => p.brand == "Filter2");
        //         Assert.Contains(priceRangeProducts, p => p.brand == "Filter3");
        //     }
        // }

        [Fact]
        public async Task SortProducts_ShouldReturnOrderedProducts()
        {
            // Arrange
            var options = GetInMemoryOptions("SortProductsTest");
            var products = new List<Product>
            {
                CreateTestProduct("SortC", 300),
                CreateTestProduct("SortA", 100),
                CreateTestProduct("SortB", 200)
            };

            // Add products
            using (var context = new AppDbContext(options))
            {
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }

            // Act & Assert - Sort by brand ascending
            using (var context = new AppDbContext(options))
            {
                var sortedByBrand = await context.Products
                    .OrderBy(p => p.brand)
                    .ToListAsync();
                
                Assert.Equal(3, sortedByBrand.Count);
                Assert.Equal("SortA", sortedByBrand[0].brand);
                Assert.Equal("SortB", sortedByBrand[1].brand);
                Assert.Equal("SortC", sortedByBrand[2].brand);
            }

            // Act & Assert - Sort by price descending
            using (var context = new AppDbContext(options))
            {
                var sortedByPrice = await context.Products
                    .OrderByDescending(p => p.price)
                    .ToListAsync();
                
                Assert.Equal(3, sortedByPrice.Count);
                Assert.Equal("SortC", sortedByPrice[0].brand);
                Assert.Equal("SortB", sortedByPrice[1].brand);
                Assert.Equal("SortA", sortedByPrice[2].brand);
            }
        }

        [Fact]
        public async Task UpdateMultipleProperties_ShouldPersistAllChanges()
        {
            // Arrange
            var options = GetInMemoryOptions("MultiUpdateTest");
            var product = CreateTestProduct("MultiTest", 500);

            // Add the product
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            // Act - Update multiple properties
            using (var context = new AppDbContext(options))
            {
                var savedProduct = await context.Products.FirstAsync(p => p.brand == "MultiTest");
                
                // Update multiple properties
                savedProduct.price = 750;
                savedProduct.color = "Green";
                savedProduct.size = "XL";
                savedProduct.waterproof = true;
                savedProduct.compartments = 5;
                
                await context.SaveChangesAsync();
            }

            // Assert - Verify all changes were persisted
            using (var context = new AppDbContext(options))
            {
                var updatedProduct = await context.Products.FirstAsync(p => p.brand == "MultiTest");
                
                Assert.Equal(750, updatedProduct.price);
                Assert.Equal("Green", updatedProduct.color);
                Assert.Equal("XL", updatedProduct.size);
                Assert.True(updatedProduct.waterproof);
                Assert.Equal(5, updatedProduct.compartments);
            }
        }

        [Fact]
        public async Task TrackedEntities_ShouldReflectChanges()
        {
            // Arrange
            var options = GetInMemoryOptions("TrackedEntitiesTest");
            var product = CreateTestProduct("TrackTest", 300);

            // Add the product and make changes while entity is tracked
            using (var context = new AppDbContext(options))
            {
                // Add and track an entity
                context.Products.Add(product);
                await context.SaveChangesAsync();
                
                // Make changes to the tracked entity
                product.price = 450;
                product.style = "Updated";
                
                // Save changes
                await context.SaveChangesAsync();
                
                // Verify changes were saved
                var updatedProduct = await context.Products.FirstAsync(p => p.brand == "TrackTest");
                Assert.Equal(450, updatedProduct.price);
                Assert.Equal("Updated", updatedProduct.style);
            }
        }

        // [Fact]
        // public async Task BulkOperations_ShouldHandleMultipleEntities()
        // {
        //     // Arrange
        //     var options = GetInMemoryOptions("BulkTest");
        //     var products = new List<Product>();
        //     
        //     // Create 10 products
        //     for (int i = 1; i <= 10; i++)
        //     {
        //         products.Add(CreateTestProduct($"Bulk{i}", i * 100));
        //     }
        //
        //     // Act - Add all products at once
        //     using (var context = new AppDbContext(options))
        //     {
        //         await context.Products.AddRangeAsync(products);
        //         var saveResult = await context.SaveChangesAsync();
        //         
        //         // Assert correct number of entities added
        //         Assert.Equal(10, saveResult);
        //     }
        //
        //     // Assert - Verify all products were added
        //     using (var context = new AppDbContext(options))
        //     {
        //         var count = await context.Products.CountAsync();
        //         Assert.Equal(10, count);
        //         
        //         // Update multiple products
        //         var productsToUpdate = await context.Products
        //             .Where(p => p.price > 500)
        //             .ToListAsync();
        //         
        //         foreach (var p in productsToUpdate)
        //         {
        //             p.waterproof = true;
        //         }
        //         
        //         await context.SaveChangesAsync();
        //     }
        //
        //     // Verify bulk update worked
        //     using (var context = new AppDbContext(options))
        //     {
        //         var waterproofCount = await context.Products
        //             .CountAsync(p => p.waterproof);
        //         
        //         Assert.Equal(5, waterproofCount); // Products with price > 500
        //     }
        // }

        #region Helper Methods

        private Product CreateTestProduct(string brand, decimal price, string size = "S", string color = "Blue")
        {
            return new Product
            {
                brand = brand,
                price = price,
                weight_capacity_kg = 5f,
                waterproof = brand.Contains("3") || brand.Contains("C"), // Just for variety
                size = size,
                color = color,
                style = brand.Contains("2") ? "Urban" : "Classic",
                compartments = brand.Length % 5 + 1 // Just for variety
            };
        }

        #endregion
    }
}