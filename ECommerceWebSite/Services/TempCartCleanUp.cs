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
    public class TempCartCleanUp : IHostedService, IDisposable
    {
        Timer timer;
        readonly IServiceScopeFactory scopeFactory;
        readonly ILogger<TempCartCleanUp> logger;
        bool isRunning;
        public TempCartCleanUp(IServiceScopeFactory scopeFactory, ILogger<TempCartCleanUp> logger)
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
            timer = new Timer(WatchOrder, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;

        }

        private void WatchOrder(object state)
        {
            if (isRunning)
                return;
            isRunning = true;

            logger.LogInformation("Temp Cart CleanUp Task Started");
            using var scope = scopeFactory.CreateScope();
            var cartServices = scope.ServiceProvider.GetRequiredService<ICartServices>();
            var count = cartServices.CleanUpTempCart();
            if (!count)
            {
                logger.LogInformation("some thing wrong in deeleting temp items cart");
            }
            isRunning = false;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(int.MaxValue, 0);
            return Task.CompletedTask;
        }
    }
}
