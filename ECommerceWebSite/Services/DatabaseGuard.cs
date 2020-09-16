using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerceWebSite.Services
{
    public class DatabaseGuard : IHostedService, IDisposable
    {
        Timer timer;
        readonly IServiceScopeFactory scopeFactory;
        readonly ILogger<DatabaseGuard> logger;
        bool isRunning;
        public DatabaseGuard(IServiceScopeFactory scopeFactory,ILogger<DatabaseGuard> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
            isRunning = false;
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(WatchOrder, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            return Task.CompletedTask;

        }

        private void WatchOrder(object state)
        {
            if (isRunning)
                return;
            isRunning = true;

            logger.LogInformation("dbGuard Task Started");
            using var scope = scopeFactory.CreateScope();
            var orderServices = scope.ServiceProvider.GetRequiredService<IOrderServices>();
            var count = orderServices.ValidateOrders();

            isRunning = false;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(int.MaxValue, 0);
            return Task.CompletedTask;
        }
    }
}
