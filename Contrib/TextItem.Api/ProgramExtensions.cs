using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RecAll.Contrib.TextItem.Api.Services;

namespace RecAll.Contrib.TextItem.Api;

public static class ProgramExtensions
{
    public static void AddCustomConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddDaprSecretStore("recall-secretstore", 
            new DaprClientBuilder().Build());
    }
    
    public static void AddCustomDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<TextItemContext>(options =>
        {
            options.UseSqlServer(builder.Configuration["ConnectionStrings:TextItemContext"]);
        });
    }
    
    public static void ApplyDatabaseMigrations(this WebApplicationBuilder builder)
    {
    }
}