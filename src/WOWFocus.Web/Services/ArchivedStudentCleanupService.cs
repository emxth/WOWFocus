using WOWFocus.Application.Interfaces;

namespace WOWFocus.Web.Services;

public class ArchivedStudentCleanupService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly IConfiguration _config;

    public ArchivedStudentCleanupService(IServiceProvider provider, IConfiguration config)
    {
        _provider = provider;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStudentService>();
            var retentionDays = _config.GetValue<int>("ArchiveRetentionDays");

            await service.PurgeExpiredArchivedAsync(retentionDays);

            // Run daily
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}