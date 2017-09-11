using System;

namespace Processes.Managers
{
    public interface ITimeManager : IDisposable
    {
        event EventHandler Tick;

        void Start();
        void Stop();
    }
}