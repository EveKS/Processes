using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Processes.Managers
{
    public class TimeManager : ITimeManager
    {
        #region head
        private const int DEFAULT_INTERVAL = 1500;
        private readonly int interval;

        private static Timer _timer;
        public event EventHandler Tick;
        #endregion

        public TimeManager() : this(DEFAULT_INTERVAL)
        { }

        public TimeManager(int interval)
        {
            this.interval = interval;

            _timer = new Timer(interval);
            _timer.Elapsed += _timer_Elapsed; ;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Tick != null)
            {
                Tick.Invoke(this, EventArgs.Empty);
            }
        }

        #region ITimeManager
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
        #endregion
    }
}
