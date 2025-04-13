using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Data;
using Xunit;

public class ProductValidationTests
{
    [Fact]
    public void Product_WithNegativePrice_ShouldFailValidation()
    {
        var product = new Product { brand = "Invalid", price = -10M, size = "M" };
        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(product, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage.Contains("between 0 and 100000"));
    }

    [Fact]
    public void Product_WithoutBrand_ShouldFailValidation()
    {
        var product = new Product { brand = "", price = 100M, size = "L" };
        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(product, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage.Contains("Brand is required"));
    }
}