using Blazored.LocalStorage;
using FakeProduct;
using FakeProduct.Models;
using FakeProduct.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("fakestore", http => http.BaseAddress = new Uri("https://fakestoreapi.com/"));
builder.Services.AddScoped<FakeStoreProductsService>();
builder.Services.AddScoped<FakeStoreCartsService>();
builder.Services.AddScoped<FakeStoreUsersService>();
builder.Services.AddScoped<FakeStoreLoginService>();

builder.Services.AddHttpClient("awesomeapi", http => http.BaseAddress = new Uri("https://economia.awesomeapi.com.br/"));
builder.Services.AddScoped<AwesomeApiService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<LocalStorageDataService>();

builder.Services.AddScoped<AuthenticationStateProvider, FakeStoreAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(sp => new CascadingValueSource<MinhasCotacoes>(new MinhasCotacoes(
        sp.GetRequiredService<AwesomeApiService>(),
        sp.GetRequiredService<ILogger<MinhasCotacoes>>()), isFixed: false));
builder.Services.AddCascadingValue(sp => sp.GetRequiredService<CascadingValueSource<MinhasCotacoes>>());

await builder.Build().RunAsync();
