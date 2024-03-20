using Dapr.Extensions.Configuration;
using Dapr.Client;
using RecAll.Contrib.TextItem.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomConfiguration();
builder.AddCustomDatabase();

Console.WriteLine(builder.Configuration["ConnectionStrings:TextItemContext"]);

var app = builder.Build();

app.Run();
