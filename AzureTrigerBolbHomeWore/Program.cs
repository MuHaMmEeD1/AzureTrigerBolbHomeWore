using AzureTrigerBolbHomeWore.Services.Abstract;
using AzureTrigerBolbHomeWore.Services.Conctere;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

builder.Services.AddScoped<IBlobService, BlobService>();

var app = builder.Build();
app.Run();
