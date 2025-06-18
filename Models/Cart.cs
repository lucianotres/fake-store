using System.ComponentModel.DataAnnotations.Schema;

namespace FakeProduct.Models;

public class Cart
{
    public int Id { get; set; } = 0;
    public int UserId { get; set; } = 0;
    public DateTime? Date { get; set; }
    public List<CartProduct> Products { get; set; } = new ();

    [NotMapped]
    public decimal Total 
    { 
        get => Products.Sum(s => s.Total); 
    }
}
