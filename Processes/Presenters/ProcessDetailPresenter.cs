using Processes.Managers;
using Processes.Services;
using Processes.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Processes.Presenters
{
    class ProcessDetailPresenter
    {
        private CancellationTokenSource _ctoken;

        private readonly ITimeManager _timeManager;
        private readonly IProcessInfoControl _view;
        private readonly ILoggerFactory _loggerFactory;

        private IProcessInfoService _processInfoService;

        public ProcessDetailPresenter(ITimeManager timeManager,
            IProcessInfoControl view, ILoggerFactory loggerFactory)
        {
            _timeManager = timeManager;
            _view = view;
            _loggerFactory = loggerFactory;

            _ctoken = new CancellationTokenSource();

            _view.ProcessInfoControlClick += _view_ProcessInfoControlClick;
            _view.ProcessInfoControlHandleDestroyed += _view_ProcessInfoControlHandleDestroyed;
        }

        private void _view_ProcessInfoControlHandleDestroyed(object sender, EventArgs e)
        {
            try
            {
                if (_processInfoService != null)
                {
                    _processInfoService.Dispose();
                }
            }
            catch (Exception ex)
            {
                _loggerFactory.ErrorLogged(ex);
            }
        }

        private void _view_ProcessInfoControlClick(object sender, EventArgs e)
        {
            try
            {
                var isOpen = sender as bool?;
                var processDetails = _view.GetProcessDetails;

                if (isOpen == true)
                {
                    _processInfoService = new ProcessInfoService(processDetails.ProcessName);
                    TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

                    _processInfoService.LoadProcessesInfo(_ctoken)
                        .ContinueWith(ant =>
                        {
                            _loggerFactory.UserOpenInfoLogged(processDetails);
                            _timeManager.Tick += _timeManager_Tick;
                        }, CancellationToken.None,
                           TaskContinuationOptions.None,
                           scheduler);
                }
                else
                {
                    _loggerFactory.UserCloseInfoLogged(processDetails);
                    _timeManager.Tick -= _timeManager_Tick;

                    if (_processInfoService != null)
                    {
                        _processInfoService.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerFactory.ErrorLogged(ex);
            }
        }

        private void _timeManager_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_processInfoService.ProcessInfo != null)
                {
                    _view.ProcessInfo = _processInfoService.ProcessInfo;
                }

                _processInfoService.LoadProcessesInfo(_ctoken);
            }
            catch (Exception ex)
            {
                _loggerFactory.ErrorLogged(ex);
            }
        }
    }
}
