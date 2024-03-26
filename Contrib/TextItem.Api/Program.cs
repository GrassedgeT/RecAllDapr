using RecAll.Contrib.TextItem.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomConfiguration();
builder.AddCustomDatabase();
builder.AddCustomApplicationServices();
builder.AddCustomSwagger();
builder.AddCustomSerilog();
builder.AddInvalidModelStateResponseFactory();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCustomSwagger();
    app.MapGet("/", () => Results.LocalRedirect("~/swagger")); 
}

app.MapControllers();
app.ApplyDatabaseMigrations();
app.Run();
