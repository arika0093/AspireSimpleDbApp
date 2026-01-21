using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspireSimpleDbApp.Database;

public static class AppDbContextExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services)
    {
        services.AddDbContextFactory<AppDbContext>(
            (provider, builder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString =
                    configuration.GetConnectionString("appdb")
                    ?? throw new InvalidOperationException("Connection string 'appdb' not found.");
                builder.UseNpgsql(connectionString);
            }
        );
        return services;
    }
}
