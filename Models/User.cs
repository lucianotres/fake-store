namespace FakeProduct.Models;
public class User
{
    public int Id { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Password { get; set; }
}
