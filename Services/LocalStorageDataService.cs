using FakeProduct.Models;

namespace FakeProduct.Services;

public class LocalStorageDataService
{
    private readonly IServiceProvider _servciceProvider;

    public List<Product> Produtos { get; private set; } = new();
    public List<User> Usuarios { get; private set; } = new();
    public List<Cart> Carrinhos { get; private set; } = new();

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

    public async Task AtualizarListaCarrinhos()
    {
        using var scope = _servciceProvider.CreateScope();
        var cartsService = scope.ServiceProvider.GetRequiredService<FakeStoreCartsService>();

        var carrinhos = await cartsService.GetCartsAsync();
        if (carrinhos != null)
        {
            Carrinhos.Clear();
            Carrinhos.AddRange(carrinhos);
        }
    }
}
