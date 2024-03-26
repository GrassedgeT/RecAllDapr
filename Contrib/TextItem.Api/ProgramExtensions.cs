using Dapr.Client;
using Dapr.Extensions.Configuration;
using Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using RecAll.Contrib.TextItem.Api.Services;
using Serilog;
using TheSalLab.GeneralReturnValues;

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
    
    public static readonly string AppName = typeof(ProgramExtensions).Namespace;
    public static void AddCustomSerilog(this WebApplicationBuilder builder) {
        var seqServerUrl = builder.Configuration["Serilog:SeqServerUrl"];

        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(builder.Configuration).WriteTo.Console().WriteTo
            .Seq(seqServerUrl).Enrich.WithProperty("ApplicationName", AppName)
            .CreateLogger();

        builder.Host.UseSerilog();
    }
    
    public static void AddInvalidModelStateResponseFactory(
        this WebApplicationBuilder builder) {
        builder.Services.AddOptions().Configure<ApiBehaviorOptions>(options => {
            options.InvalidModelStateResponseFactory = context =>
                new OkObjectResult(ServiceResult
                    .CreateInvalidParameterResult(
                        new ValidationProblemDetails(context.ModelState).Errors
                            .Select(p =>
                                $"{p.Key}: {string.Join(" / ", p.Value)}"))
                    .ToServiceResultViewModel());
        });
    }
    
    public static void
        AddCustomHealthChecks(this WebApplicationBuilder builder) =>
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy()).AddDapr()
            .AddSqlServer(builder.Configuration["ConnectionStrings:TextItemContext"]!,
                name: "TextListDb-check", tags: new[] { "TextListDb" });

}