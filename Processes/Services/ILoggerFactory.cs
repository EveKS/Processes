using System;
using Processes.Models;

namespace Processes.Services
{
    interface ILoggerFactory
    {
        void AddedProcessesLogged(params ProcessDetails[] processDetails);
        void CloseProgramLogged();
        void ErrorLogged(Exception ex);
        void UserCloseInfoLogged(ProcessDetails processDetails);
        void RemovedProcessesLogged(params ProcessDetails[] processDetails);
        void RunProgramLogged();
        void UserOpenInfoLogged(ProcessDetails processDetails);
    }
}