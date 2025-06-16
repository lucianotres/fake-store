namespace FakeProduct.Models;

public class Cart
{
    public int Id { get; set; } = 0;
    public int UserId { get; set; } = 0;
    public List<Product> Products { get; set; } = new ();

}
