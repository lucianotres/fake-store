namespace FakeProduct.Models;

public class Cart
{
    public int Id { get; set; } = 0;
    public int UserId { get; set; } = 0;
    public DateTime? Date { get; set; }
    public List<CartProduct> Products { get; set; } = new ();
}
