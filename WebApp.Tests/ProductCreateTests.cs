using Xunit;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace WebApp.Tests
{
    public class ProductCreateTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly AppDbContext _context;

        public ProductCreateTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"ProductTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(_options);
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void Product_WithValidData_ShouldBeValid()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Empty(results);
        }

        // [Theory]
        // [InlineData("", "Brand is required.")]
        // [InlineData("A", "Brand must be at least 2 characters")]
        // [InlineData("ThisBrandNameIsFarTooLongAndExceedsTheMaximumAllowedCharacterLimitForThisField", "Brand cannot exceed 50 characters")]
        // public void Product_WithInvalidBrand_ShouldBeInvalid(string brand, string expectedError)
        // {
        //     // Arrange
        //     var product = CreateValidProduct();
        //     product.brand = brand;
        //
        //     // Act
        //     var results = ValidateModel(product);
        //
        //     // Assert
        //     Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains(expectedError));
        // }

        [Theory]
        [InlineData(-1, "Price must be between 0 and 100000.")]
        [InlineData(100001, "Price must be between 0 and 100000.")]
        public void Product_WithInvalidPrice_ShouldBeInvalid(decimal price, string expectedError)
        {
            // Arrange
            var product = CreateValidProduct();
            product.price = price;

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains(expectedError));
        }

        // [Theory]
        // [InlineData(0, "Weight capacity must be greater than 0")]
        // [InlineData(-1, "Weight capacity must be greater than 0")]
        // [InlineData(501, "Weight capacity cannot exceed 500")]
        
        [Theory]
        [InlineData(-1, "The field weight_capacity_kg must be between 0 and 1000.")]
        [InlineData(1001, "The field weight_capacity_kg must be between 0 and 1000.")]

        public void Product_WithInvalidWeightCapacity_ShouldBeInvalid(decimal weightCapacity, string expectedError)
        {
            // Arrange
            var product = CreateValidProduct();
            product.weight_capacity_kg = (float)weightCapacity;

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains(expectedError));
        }

        // [Theory]
        // [InlineData("", "Size is required")]
        // [InlineData("ThisSizeIsTooLongAndShouldFailValidation", "Size cannot exceed 20 characters")]
        // public void Product_WithInvalidSize_ShouldBeInvalid(string size, string expectedError)
        // {
        //     // Arrange
        //     var product = CreateValidProduct();
        //     product.size = size;
        //
        //     // Act
        //     var results = ValidateModel(product);
        //
        //     // Assert
        //     Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains(expectedError));
        // }

        // [Theory]
        // // [InlineData("", "Color is required")]
        // [InlineData("ThisColorNameIsTooLongAndShouldFailValidation", "Color cannot exceed 30 characters")]
        // public void Product_WithInvalidColor_ShouldBeInvalid(string color, string expectedError)
        // {
        //     // Arrange
        //     var product = CreateValidProduct();
        //     product.color = color;
        //
        //     // Act
        //     var results = ValidateModel(product);
        //
        //     // Assert
        //     Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains(expectedError));
        // }

        // [Theory]
        // [InlineData("", "Style is required")]
        // [InlineData("ThisStyleNameIsTooLongAndShouldFailValidation", "Style cannot exceed 30 characters")]
        // public void Product_WithInvalidStyle_ShouldBeInvalid(string style, string expectedError)
        // {
        //     // Arrange
        //     var product = CreateValidProduct();
        //     product.style = style;
        //
        //     // Act
        //     var results = ValidateModel(product);
        //
        //     // Assert
        //     Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains(expectedError));
        // }

        [Theory]
        // [InlineData(0, "Compartments must be greater than 0")]
        // [InlineData(-1, "Compartments must be greater than 0")]
        // [InlineData(51, "Compartments cannot exceed 50")]
        [InlineData(-1, "The field compartments must be between 0 and 20.")]
        [InlineData(21, "The field compartments must be between 0 and 20.")]

        public void Product_WithInvalidCompartments_ShouldBeInvalid(int compartments, string expectedError)
        {
            // Arrange
            var product = CreateValidProduct();
            product.compartments = compartments;

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Contains(results, r => r.ErrorMessage != null && r.ErrorMessage.Contains(expectedError));
        }

        [Fact]
        public async Task HandleValidSubmit_ShouldAddProductToDatabase()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Assert
            var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.brand == product.brand);
            Assert.NotNull(savedProduct);
            Assert.Equal(product.price, savedProduct.price);
            Assert.Equal(product.weight_capacity_kg, savedProduct.weight_capacity_kg);
            Assert.Equal(product.waterproof, savedProduct.waterproof);
            Assert.Equal(product.size, savedProduct.size);
            Assert.Equal(product.color, savedProduct.color);
            Assert.Equal(product.style, savedProduct.style);
            Assert.Equal(product.compartments, savedProduct.compartments);
        }

        [Fact]
        public async Task HandleValidSubmit_WithMultipleProducts_ShouldAddAllToDatabase()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    brand = "Brand1",
                    price = 100,
                    weight_capacity_kg = 5,
                    waterproof = true,
                    size = "S",
                    color = "Red",
                    style = "Casual",
                    compartments = 3
                },
                new Product
                {
                    brand = "Brand2",
                    price = 200,
                    weight_capacity_kg = 10,
                    waterproof = false,
                    size = "M",
                    color = "Blue",
                    style = "Formal",
                    compartments = 5
                },
                new Product
                {
                    brand = "Brand3",
                    price = 300,
                    weight_capacity_kg = 15,
                    waterproof = true,
                    size = "L",
                    color = "Green",
                    style = "Sport",
                    compartments = 7
                }
            };

            // Act
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Assert
            Assert.Equal(3, await _context.Products.CountAsync());
            Assert.NotNull(await _context.Products.FirstOrDefaultAsync(p => p.brand == "Brand1"));
            Assert.NotNull(await _context.Products.FirstOrDefaultAsync(p => p.brand == "Brand2"));
            Assert.NotNull(await _context.Products.FirstOrDefaultAsync(p => p.brand == "Brand3"));
        }

        // [Fact]
        // public async Task Product_WithDuplicateId_ShouldThrowException()
        // {
        //     // Arrange
        //     var product1 = CreateValidProduct();
        //     product1.id = 1;
        //
        //     var product2 = CreateValidProduct();
        //     product2.brand = "AnotherBrand";
        //     product2.id = 1; // Same ID as product1
        //
        //     // Act & Assert
        //     _context.Products.Add(product1);
        //     await _context.SaveChangesAsync();
        //
        //     _context.Products.Add(product2);
        //     await Assert.ThrowsAsync<DbUpdateException>(() => _context.SaveChangesAsync());
        // }

        [Fact]
        public async Task HandleValidSubmit_WithEdgeCaseValues_ShouldAddToDatabase()
        {
            // Arrange
            var product = new Product
            {
                brand = "EdgeCase",
                price = 0.01m, // Minimum valid price
                weight_capacity_kg = (float)0.1m, // Very small weight
                waterproof = true,
                size = "XS",
                color = "Transparent",
                style = "Minimalist",
                compartments = 1 // Minimum valid compartments
            };

            // Act
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Assert
            var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.brand == "EdgeCase");
            Assert.NotNull(savedProduct);
            Assert.Equal(0.01m, savedProduct.price);
            Assert.Equal((float)0.1m, savedProduct.weight_capacity_kg);
            Assert.Equal(1, savedProduct.compartments);
        }

        [Fact]
        public async Task HandleValidSubmit_WithUnicodeCharacters_ShouldAddToDatabase()
        {
            // Arrange
            var product = new Product
            {
                brand = "ブランド", // Japanese for "brand"
                price = 1500,
                weight_capacity_kg = 8,
                waterproof = true,
                size = "中", // Japanese for "medium"
                color = "青", // Japanese for "blue"
                style = "カジュアル", // Japanese for "casual"
                compartments = 4
            };

            // Act
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Assert
            var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.brand == "ブランド");
            Assert.NotNull(savedProduct);
            Assert.Equal("青", savedProduct.color);
            Assert.Equal("カジュアル", savedProduct.style);
        }

        #region Helper Methods
        private static Product CreateValidProduct()
        {
            return new Product
            {
                brand = "TestBrand",
                price = 888,
                weight_capacity_kg = 8,
                waterproof = true,
                size = "M",
                color = "Black",
                style = "Tote",
                compartments = 4
            };
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
        #endregion
    }
}