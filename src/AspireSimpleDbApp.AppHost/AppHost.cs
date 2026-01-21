var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithImage("postgres:16-alpine")
    .WithPgWeb()
    .WithDataVolume();

var database = postgres.AddDatabase("appdb");

var dbMigration = builder
    .AddProject<Projects.AspireSimpleDbApp_DbMigration>("dbmigration")
    .WithReference(database)
    .WaitFor(database);

var apiService = builder
    .AddProject<Projects.AspireSimpleDbApp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WaitForCompletion(dbMigration);

builder
    .AddProject<Projects.AspireSimpleDbApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WithReference(database)
    .WaitFor(apiService);

builder
    .AddProject<Projects.AspireSimpleDbApp_BotWorker>("botworker")
    .WithReference(database)
    .WaitForCompletion(dbMigration);

builder.Build().Run();
