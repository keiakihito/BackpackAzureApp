using System.ComponentModel.DataAnnotations;

namespace WebApp.Data;

public class Product
{
    public int id { get; set; }

    [Required]
    public string brand { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal price { get; set; }

    [Range(0, 1000)]
    public float weight_capacity_kg { get; set; }

    public bool waterproof { get; set; }

    [Required]
    public string size { get; set; } = string.Empty;

    public string color { get; set; } = string.Empty;
    public string style { get; set; } = string.Empty;

    [Range(0, 20)]
    public int compartments { get; set; }
    
    
}