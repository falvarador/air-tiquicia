using AirTiquicia.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress), Timeout = TimeSpan.FromSeconds(30), DefaultRequestHeaders = { { "Accept-Language", "es-MX" } } });
builder.Services.AddApiAuthorization();

builder.Services.AddScoped<JWTAuthenticationProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationProvider>(
    provider => provider.GetRequiredService<JWTAuthenticationProvider>());
builder.Services.AddScoped<ILoginService, JWTAuthenticationProvider>(
    provider => provider.GetRequiredService<JWTAuthenticationProvider>());

await builder.Build().RunAsync();
