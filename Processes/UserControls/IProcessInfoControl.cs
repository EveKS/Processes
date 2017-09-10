using Processes.Models;
using System;

namespace Processes.UserControls
{
    public interface IProcessInfoControl
    {
        ProcessDetails GetProcessDetails { get; }
        ProcessInfo ProcessInfo { set; }

        event EventHandler ProcessInfoControlClick;
        event EventHandler ProcessInfoControlHandleDestroyed;
    }
}