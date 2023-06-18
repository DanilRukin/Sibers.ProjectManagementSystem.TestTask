using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Sibers.ProjectManagementSystem.Presentation.Blazor;
using System.Security.Claims;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


IConfiguration configuration = builder.Configuration;
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("", client =>
{
    client.BaseAddress = new Uri(configuration["ApiBaseAddress"], UriKind.Absolute);
});


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOptions();  // для авторизации
builder.Services.AddAuthorizationCore(); // для авторизации
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>(); // для авторизации



builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(Marker).Assembly));
builder.Services.AddAutoMapper(typeof(Marker).Assembly);

await builder.Build().RunAsync();


internal class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity anonymousClaim = new ClaimsIdentity();
        ClaimsPrincipal anonymousPrincipal = new ClaimsPrincipal(anonymousClaim);
        return Task.FromResult(new AuthenticationState(anonymousPrincipal));
    }
}
