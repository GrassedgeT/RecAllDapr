using HealthChecks.UI.Client;
using Infrastructure.Api;
using RecAll.Contrib.TextItem.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomConfiguration();
builder.AddCustomDatabase();
builder.AddCustomApplicationServices();
builder.AddCustomSwagger();
builder.AddCustomHealthChecks();
builder.AddCustomSerilog();
builder.AddInvalidModelStateResponseFactory();

builder.Services.AddDaprClient();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCustomSwagger();
    app.MapGet("/", () => Results.LocalRedirect("~/swagger")); 
}

app.MapCustomHealthChecks(
    responseWriter: UIResponseWriter.WriteHealthCheckUIResponse);

app.MapControllers();
app.ApplyDatabaseMigrations();
app.Run();
