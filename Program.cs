using Blazored.LocalStorage;
using FakeProduct;
using FakeProduct.Services;
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

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<LocalStorageDataService>();

builder.Services.AddScoped<AuthenticationStateProvider, FakeStoreAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
