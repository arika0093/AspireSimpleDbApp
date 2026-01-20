using AspireSimpleDbApp.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddAppDbContext();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.MapGet(
        "/products",
        async (AppDbContext dbContext) =>
        {
            return dbContext.Products.SelectExpr<Product, ProductDto>(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                Price = (double)x.Price,
                x.CreatedAt,
            });
        }
    )
    .WithName("GetProducts");

app.MapDefaultEndpoints();

app.Run();
