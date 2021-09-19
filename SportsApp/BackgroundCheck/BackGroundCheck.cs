using Microsoft.Extensions.Hosting;
using SportsApp.BackgroundCheck.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsApp.BackgroundCheck
{
    public class BackGroundCheck : BackgroundService
    {
        private readonly IWorker worker;

        public BackGroundCheck(IWorker worker)
        {
            this.worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await worker.DoWork(stoppingToken);
        }
    }
    
}
