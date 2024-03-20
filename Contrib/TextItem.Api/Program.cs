using Dapr.Extensions.Configuration;
using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDaprSecretStore("recall-secretstore", 
    new DaprClientBuilder().Build());
Console.WriteLine(builder.Configuration["ConnectionStrings:TextItemContext"]);

var app = builder.Build();

app.Run();
