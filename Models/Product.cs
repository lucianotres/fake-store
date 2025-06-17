using System.ComponentModel.DataAnnotations;

namespace FakeProduct.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Obrigatório informar um título!")]
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}
