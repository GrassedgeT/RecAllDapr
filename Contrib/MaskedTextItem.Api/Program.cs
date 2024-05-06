using RecAll.Contrib.MaskedTextItem.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomConfiguration();
builder.AddCustomDatabase();

builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();

var app = builder.Build();

app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();

app.ApplyDatabaseMigrations();
app.Run();
