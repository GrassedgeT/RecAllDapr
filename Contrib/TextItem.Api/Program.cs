using RecAll.Contrib.TextItem.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomConfiguration();
builder.AddCustomDatabase();
builder.AddCustomApplicationServices();
builder.AddCustomSwagger();

builder.Services.AddControllers();
Console.WriteLine(builder.Configuration["ConnectionStrings:TextItemContext"]);

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
