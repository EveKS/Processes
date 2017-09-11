using System.Threading;
using System.Threading.Tasks;
using Processes.Models;

namespace Processes.Services
{
    public interface IProcessInfoService
    {
        ProcessInfo ProcessInfo { get; }

        void Dispose();
        Task LoadProcessesInfo(CancellationTokenSource ctoken = null);
    }
}