using System;

namespace Processes.Managers
{
    interface ITimeManager
    {
        event EventHandler Tick;

        void Dispose();
        void Start();
        void Stop();
    }
}