using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace AspireUrlShortener.Api.UrlShortening.Database;

internal sealed class UrlShorteningDbInitializer(IServiceProvider serviceProvider, ILogger<UrlShorteningDbInitializer> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await InitializeDatabaseAsync(dbContext, stoppingToken);
    }

    private async Task InitializeDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        using var activity = _activitySource.StartActivity("Migrating history database", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        await strategy.ExecuteAsync(() => dbContext.Database.MigrateAsync(cancellationToken));

        await SeedAsync(dbContext, cancellationToken);

        logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }

    private async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        if (!dbContext.ShortenedUrls.Any())
        {
            logger.LogInformation("Seeded ShortenedUrls table");
        }
    }
}

internal sealed class HistoryDbInitializerHealthCheck(UrlShorteningDbInitializer dbInitializer) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var task = dbInitializer.ExecuteTask;

        return task switch
        {
            { IsCompletedSuccessfully: true } => Task.FromResult(HealthCheckResult.Healthy()),
            { IsFaulted: true } => Task.FromResult(HealthCheckResult.Unhealthy(task.Exception?.InnerException?.Message, task.Exception)),
            { IsCanceled: true } => Task.FromResult(HealthCheckResult.Unhealthy("Database initialization was canceled")),
            _ => Task.FromResult(HealthCheckResult.Degraded("Database initialization is still in progress"))
        };
    }
}