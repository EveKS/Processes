using System;
using Processes.Models;

namespace Processes
{
    public interface IMainForm
    {
        ProcessInfo ProcessInfo { set; }

        event EventHandler AddProcessInfo;
        event EventHandler MainFormFormClosed;
        event EventHandler MainFormLoad;

        void AddGetNICLabel(ProcessInfo processInfo);
        void AddNewProcess(ProcessDetails[] processDetails);
        void RemoveProcess(ProcessDetails[] processDetails);
    }
}