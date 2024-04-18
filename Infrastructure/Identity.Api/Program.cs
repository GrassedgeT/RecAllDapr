using RecAll.Infrastructure.Identity.Api;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddIdentityServer()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.Clients).AddDeveloperSigningCredential();

var app = builder.Build();

app.UseIdentityServer();

app.Run();