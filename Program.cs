using FakeProduct;
using FakeProduct.Services;
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

await builder.Build().RunAsync();
