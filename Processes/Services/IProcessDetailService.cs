using Processes.Models;

namespace Processes.Services
{
    public interface IProcessDetailService
    {
        ProcessDetails[] GetNewProcesses();
        ProcessDetails[] GetRemoveProcess();
    }
}