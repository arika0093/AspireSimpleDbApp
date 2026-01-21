using AspireSimpleDbApp.Database;
using Microsoft.EntityFrameworkCore;

namespace AspireSimpleDbApp.DbMigration;

internal class MigrationWorker(
    IDbContextFactory<AppDbContext> dbContextFactory,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<MigrationWorker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Starting database migration...");

            using var dbContext = dbContextFactory.CreateDbContext();

            // Apply pending migrations
            await dbContext.Database.MigrateAsync(stoppingToken);
            logger.LogInformation("Database migration completed successfully");

            // Seed initial data if needed
            await SeedDataAsync(dbContext, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database migration");
            throw;
        }

        // Stop the worker after migration completes
        hostApplicationLifetime.StopApplication();
    }

    private async Task SeedDataAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Products.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Database already seeded");
            return;
        }

        logger.LogInformation("Seeding initial data...");

        var products = new[]
        {
            new Product
            {
                Name = "Laptop",
                Description = "High-performance laptop",
                Price = 999.99m,
            },
            new Product
            {
                Name = "Mouse",
                Description = "Wireless mouse",
                Price = 29.99m,
            },
            new Product
            {
                Name = "Keyboard",
                Description = "Mechanical keyboard",
                Price = 79.99m,
            },
            new Product
            {
                Name = "Monitor",
                Description = "4K display monitor",
                Price = 399.99m,
            },
        };

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Initial data seeding completed");
    }
}
