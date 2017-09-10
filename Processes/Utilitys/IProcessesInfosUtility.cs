using Processes.Models;

namespace Processes.Utilitys
{
    interface IProcessesInfosUtility
    {
        ProcessDetails this[int index] { get; }

        int Length { get; }

        void LoadProcessesNames();
    }
}