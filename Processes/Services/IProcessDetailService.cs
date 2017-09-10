using Processes.Models;

namespace Processes.Services
{
    interface IProcessDetailService
    {
        ProcessDetails[] GetNewProcesses();
        ProcessDetails[] GetRemoveProcess();
    }
}