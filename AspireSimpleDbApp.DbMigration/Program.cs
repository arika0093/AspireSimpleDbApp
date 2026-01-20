using AspireSimpleDbApp.Database;
using AspireSimpleDbApp.DbMigration;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAppDbContext();

builder.Services.AddHostedService<MigrationWorker>();

var host = builder.Build();
await host.RunAsync();
