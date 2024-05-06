using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Polly;
using RecAll.Contrib.MaskedTextItem.Api.Services;

namespace RecAll.Contrib.MaskedTextItem.Api;

public static class ProgramExtensions
{
   public static readonly string AppName = typeof(ProgramExtensions).Namespace;
   
   public static void AddCustomConfiguration(
       this WebApplicationBuilder builder)
   {
       builder.Configuration.AddDaprSecretStore("recall-secretstore",
           new DaprClientBuilder().Build());
   }
   
   public static void AddCustomDatabase(this WebApplicationBuilder builder)
   {
       builder.Services.AddDbContext<MaskedTextItemContext>(options =>
           options.UseSqlServer(builder.Configuration["ConnectionStrings:MaskedTextItemContext"]));
   }

   public static void ApplyDatabaseMigrations(this WebApplication app)
   {
       using var scope = app.Services.CreateScope();
       var retryPolicy = CreateRetryPolicy();
       var context = scope.ServiceProvider.GetRequiredService<MaskedTextItemContext>();
       retryPolicy.Execute(context.Database.Migrate);
   }
   
   private static Policy CreateRetryPolicy()
   {    
         return Policy.Handle<Exception>().WaitAndRetryForever(
             sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
             onRetry: (exception, retry, _) => {
                 Console.WriteLine($"Retry {retry} due to {exception.Message}");
             });
   }
}