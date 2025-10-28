using Core.Entities;
using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.BackgroundServices
{
    public class UserCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UserCleanupService> _logger;

        public UserCleanupService(IServiceProvider serviceProvider, ILogger<UserCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<TSTDBContext>();
                        var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<APIUser>>();

                        _logger.LogInformation("Running user cleanup at {time}", DateTime.UtcNow);

                        // Delete unconfirmed users (older than 5 minutes)
                        var rowsAffected = await db.Database.ExecuteSqlRawAsync(@"
                        DELETE FROM Devices
                        WHERE APIUserId IN (
                            SELECT Id FROM AspNetUsers
                            WHERE PhoneNumberConfirmed = 0
                                and StudentNumber is not null
                              AND DATEDIFF(MINUTE, CreatedAt, GETUTCDATE()) > 5
                        );

                        DELETE FROM AspNetUsers
                        WHERE PhoneNumberConfirmed = 0
                            and StudentNumber is not null
                          AND DATEDIFF(MINUTE, CreatedAt, GETUTCDATE()) > 5;
                    ");
                        _logger.LogInformation("Deleted {rows} unconfirmed users", rowsAffected);
                        _logger.LogInformation("Cleanup done at {time}", DateTime.UtcNow);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during user cleanup");
                }

                await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
            }
        }
    }


}
