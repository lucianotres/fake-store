using System.ComponentModel.DataAnnotations.Schema;

namespace FakeProduct.Models;

public class CartProduct
{
    public int ProductId { get; set; } = 0;
    public int Quantity { get; set; } = 0;

    [NotMapped]
    public Product? Produto { get; set; }

    [NotMapped]
    public decimal Total 
    {
        get => (Produto?.Price ?? 0) * Quantity;
    }
}
