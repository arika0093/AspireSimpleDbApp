using AspireSimpleDbApp.BotWorker;
using AspireSimpleDbApp.Database;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAppDbContext();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
