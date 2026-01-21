using AspireSimpleDbApp.Database;
using Microsoft.EntityFrameworkCore;

namespace AspireSimpleDbApp.BotWorker;

public class Worker(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<Worker> logger)
    : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(1));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // 1秒に1回、DBに適当なデータを追加する
            using var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
            var prod = GenerateRandomProductData();
            dbContext.Products.Add(prod);
            await dbContext.SaveChangesAsync(stoppingToken);
            logger.LogInformation(
                $"Added a new product to the database. ID: {prod.Id:D}, Name: {prod.Name}"
            );
            // 次のTickまで待機
            await _timer.WaitForNextTickAsync(stoppingToken);
        }
    }

    private Product GenerateRandomProductData()
    {
        var random = new Random();
        return new Product
        {
            Name = "Product " + random.Next(1, 1000),
            Description = "Description " + random.Next(1, 1000),
            Price = (decimal)(random.NextDouble() * 1000),
            CreatedAt = DateTime.UtcNow,
        };
    }
}
