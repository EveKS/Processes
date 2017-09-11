using System.Threading;
using System.Threading.Tasks;
using Processes.Models;
using System;

namespace Processes.Services
{
    public interface IProcessInfoService : IDisposable
    {
        ProcessInfo ProcessInfo { get; }

        Task LoadProcessesInfo(CancellationTokenSource ctoken = null);
    }
}