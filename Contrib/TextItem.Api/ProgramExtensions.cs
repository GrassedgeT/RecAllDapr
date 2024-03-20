using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Polly;
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
    
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var retryPolicy = CreateRetryPolicy();
        var context =
            scope.ServiceProvider.GetRequiredService<TextItemContext>();

        retryPolicy.Execute(context.Database.Migrate);
    }
    
    private static Policy CreateRetryPolicy() {
        return Policy.Handle<Exception>().WaitAndRetryForever(
            sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
            onRetry: (exception, retry, _) => {
                Console.WriteLine(
                    "Exception {0} with message {1} detected during database migration (retry attempt {2})",
                    exception.GetType().Name, exception.Message, retry);
            });
    }
    
    public static void AddCustomApplicationServices(
        this WebApplicationBuilder builder) {
        builder.Services.AddScoped<IIdentityService, MockIdentityService>();
    }
    
    public static void AddCustomSwagger(this WebApplicationBuilder builder) =>
        builder.Services.AddSwaggerGen();
    
    public static void UseCustomSwagger(this WebApplication app) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}