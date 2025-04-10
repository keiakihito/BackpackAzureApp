using System.ComponentModel.DataAnnotations;

namespace WebApp.Data;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Range(0, 1000)]
    public float WeightCapacityKg { get; set; }

    public bool Waterproof { get; set; }

    [Required]
    public string Size { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;

    [Range(0, 20)]
    public int Compartments { get; set; }

    public DateTime ReleaseDate { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;
}