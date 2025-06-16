using FakeProduct.Models;

namespace FakeProduct.Services;

public class LocalStorageDataService
{
    private readonly IServiceProvider _servciceProvider;

    public List<Product> Produtos { get; set; } = new();
    public List<User> Usuarios { get; set; } = new();

    public LocalStorageDataService(IServiceProvider serviceProvider)
    {
        _servciceProvider = serviceProvider;
    }

    public async Task AtualizarListaProdutos()
    {
        using var scope = _servciceProvider.CreateScope();
        var productsService = scope.ServiceProvider.GetRequiredService<FakeStoreProductsService>();

        var produtos = await productsService.GetProductsAsync();
        if (produtos != null)
        {
            Produtos.Clear();
            Produtos.AddRange(produtos);
        }
    }

    public async Task AtualizarListaUsuarios()
    {
        using var scope = _servciceProvider.CreateScope();
        var usersService = scope.ServiceProvider.GetRequiredService<FakeStoreUsersService>();

        var usuarios = await usersService.GetUsersAsync();
        if (usuarios != null)
        {
            Usuarios.Clear();
            Usuarios.AddRange(usuarios);
        }
    }
}
