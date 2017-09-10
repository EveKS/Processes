using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Processes.Managers
{
    class TimeManager : IDisposable, ITimeManager
    {
        private const int INTERVAL = 1500;

        private static Timer _timer = new Timer(INTERVAL);
        public event EventHandler Tick;

        public TimeManager()
        {
            _timer.Elapsed += _timer_Elapsed; ;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Tick != null)
            {
                Tick.Invoke(this, EventArgs.Empty);
            }
        }

        void ITimeManager.Start()
        {
            _timer.Start();
        }

        void ITimeManager.Stop()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
