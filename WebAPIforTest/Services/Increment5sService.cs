using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace WebAPIforTest.Services
{
    public class Increment5sService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public Increment5sService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    try
                    {
                        var record = await db.Counters.FirstAsync(x => x.Id == 1);
                        record.Value++;
                        db.SaveChanges();
                    }
                    catch (TaskCanceledException exception)
                    {
                    }
                }
                await Task.Delay(5000);
            }

            await Task.CompletedTask;
        }
    }
}
