using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsApp.BackgroundCheck.Worker
{
    public interface IWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
