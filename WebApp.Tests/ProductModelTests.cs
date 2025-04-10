using WebApp.Data;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace WebApp.Tests
{
    public class ProductModelTests
    {
        [Fact]
        public void Product_WithValidData_ShouldBeValid()
        {
            // Arrange
            var product = new Product
            {
                brand = "TestBrand",
                price = 1000,
                weight_capacity_kg = 10.5f,
                waterproof = true,
                size = "M",
                color = "Red",
                style = "Sporty",
                compartments = 3
            };

            // Act
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Product_MissingRequiredFields_ShouldBeInvalid()
        {
            // Arrange
            var product = new Product(); // All empty fields

            // Act
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("brand"));
            Assert.Contains(results, r => r.MemberNames.Contains("size"));
        }

        [Theory]
        [InlineData("", "Brand is required.")]
        [InlineData("A", "Brand must be between 2 and 50 characters.")]
        [InlineData("ThisBrandNameIsTooLongAndShouldExceedTheMaximumLengthAllowedForBrands", "Brand must be between 2 and 50 characters.")]
        public void Product_InvalidBrand_ShouldBeInvalid(string invalidBrand, string expectedErrorMessage)
        {
            // Arrange
            var product = CreateValidProduct();
            product.brand = invalidBrand;

            // Act
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("brand") && r.ErrorMessage == expectedErrorMessage);
        }

        [Theory]
        [InlineData(-1, "Price must be between 0 and 100000.")]
        [InlineData(100001, "Price must be between 0 and 100000.")]
        public void Product_InvalidPrice_ShouldBeInvalid(decimal invalidPrice, string expectedErrorMessage)
        {
            // Arrange
            var product = CreateValidProduct();
            product.price = invalidPrice;

            // Act
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("price") && r.ErrorMessage == expectedErrorMessage);
        }

        // [Theory]
        // [InlineData(-0.1f)]
        // [InlineData(1001f)]
        // public void Product_InvalidWeightCapacity_ShouldBeInvalid(float invalidWeight)
        // {
        //     // Arrange
        //     var product = CreateValidProduct();
        //     product.weight_capacity_kg = invalidWeight;
        //
        //     // Act
        //     var results = new List<ValidationResult>();
        //     var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);
        //
        //     // Assert - only verify validation fails, don't check message content
        //     Assert.False(isValid);
        //     Assert.Contains(results, r => r.MemberNames.Contains("weight_capacity_kg"));
        // }

        [Fact]
        public void Product_EmptySize_ShouldBeInvalid()
        {
            // Arrange
            var product = CreateValidProduct();
            product.size = "";

            // Act
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("size") && r.ErrorMessage == "The size field is required.");
        }

        [Theory]
        [InlineData(-1, "The field compartments must be between 0 and 20.")]
        [InlineData(21, "The field compartments must be between 0 and 20.")]
        public void Product_InvalidCompartments_ShouldBeInvalid(int invalidCompartments, string expectedErrorMessage)
        {
            // Arrange
            var product = CreateValidProduct();
            product.compartments = invalidCompartments;

            // Act
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("compartments") && r.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public void Product_WithMultipleValidationErrors_ShouldCollectAllErrors()
        {
            // Arrange
            var product = new Product
            {
                brand = "",           // Invalid: Empty
                price = -50,          // Invalid: Negative
                weight_capacity_kg = -5.0f, // Invalid: Negative
                size = "",            // Invalid: Empty
                compartments = 25     // Invalid: Too many
            };

            // Act
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, new ValidationContext(product), results, true);

            // Assert
            Assert.False(isValid);
            Assert.Equal(5, results.Count); // Should have 5 validation errors
        }

        [Fact]
        public void Product_AllRequiredPropertiesAreDecorated()
        {
            // This test ensures that required properties have the [Required] attribute

            // Get all properties that are marked as required in the model
            var requiredProperties = new[] { "brand", "size" };

            foreach (var propName in requiredProperties)
            {
                PropertyInfo propertyInfo = typeof(Product).GetProperty(propName);
                Assert.NotNull(propertyInfo); // Property exists
                
                var requiredAttr = propertyInfo.GetCustomAttribute<RequiredAttribute>();
                Assert.NotNull(requiredAttr); // Property has [Required] attribute
            }
        }

        [Fact]
        public void Product_NumericPropertiesHaveRangeValidation()
        {
            // This test ensures that numeric properties have appropriate range validation

            var numericProperties = new Dictionary<string, (Type Type, object Min, object Max)>
            {
                { "price", (typeof(decimal), 0m, 100000m) },
                { "weight_capacity_kg", (typeof(float), 0f, 1000f) },
                { "compartments", (typeof(int), 0, 20) }
            };

            foreach (var prop in numericProperties)
            {
                PropertyInfo propertyInfo = typeof(Product).GetProperty(prop.Key);
                Assert.NotNull(propertyInfo); // Property exists
                
                var rangeAttr = propertyInfo.GetCustomAttribute<RangeAttribute>();
                Assert.NotNull(rangeAttr); // Property has [Range] attribute
                
                // Check if the range values match expected values
                Assert.Equal(prop.Value.Min.ToString(), rangeAttr.Minimum.ToString());
                Assert.Equal(prop.Value.Max.ToString(), rangeAttr.Maximum.ToString());
            }
        }

        [Fact]
        public void Product_StringPropertiesHaveAppropriateValidation()
        {
            // This test ensures that string properties have appropriate validation

            // Check brand has StringLength attribute
            PropertyInfo brandProperty = typeof(Product).GetProperty("brand");
            Assert.NotNull(brandProperty); // Property exists
            
            var stringLengthAttr = brandProperty.GetCustomAttribute<StringLengthAttribute>();
            Assert.NotNull(stringLengthAttr); // Property has [StringLength] attribute
            Assert.Equal(50, stringLengthAttr.MaximumLength);
            Assert.Equal(2, stringLengthAttr.MinimumLength);
        }
        
        [Fact]
        public void Product_CanSetAndGetProperties()
        {
            // Arrange
            var product = new Product();
            
            // Act
            product.id = 1;
            product.brand = "TestBrand";
            product.price = 999.99m;
            product.weight_capacity_kg = 15.5f;
            product.waterproof = true;
            product.size = "XL";
            product.color = "Black";
            product.style = "Modern";
            product.compartments = 5;
            
            // Assert
            Assert.Equal(1, product.id);
            Assert.Equal("TestBrand", product.brand);
            Assert.Equal(999.99m, product.price);
            Assert.Equal(15.5f, product.weight_capacity_kg);
            Assert.True(product.waterproof);
            Assert.Equal("XL", product.size);
            Assert.Equal("Black", product.color);
            Assert.Equal("Modern", product.style);
            Assert.Equal(5, product.compartments);
        }

        [Fact]
        public void Product_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var product = new Product();
            
            // Assert
            Assert.Equal(string.Empty, product.brand);
            Assert.Equal(string.Empty, product.size);
            Assert.Equal(string.Empty, product.color);
            Assert.Equal(string.Empty, product.style);
            Assert.Equal(0, product.price);
            Assert.Equal(0, product.weight_capacity_kg);
            Assert.Equal(0, product.compartments);
            Assert.False(product.waterproof);
        }

        // Helper method to create a valid product for testing
        private Product CreateValidProduct()
        {
            return new Product
            {
                brand = "TestBrand",
                price = 1000,
                weight_capacity_kg = 10.5f,
                waterproof = true,
                size = "M",
                color = "Red",
                style = "Sporty",
                compartments = 3
            };
        }
    }
}