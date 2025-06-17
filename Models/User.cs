using System.ComponentModel.DataAnnotations;

namespace FakeProduct.Models;
public class User
{
    public int Id { get; set; } = 0;

    [Required(ErrorMessage = "Obrigatório informar nome de usuário!")]
    public string Username { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "e-mail inválido!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Obrigatório informar o e-mail!")]
    public string? Password { get; set; }
}
