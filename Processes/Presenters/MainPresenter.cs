using Processes.Managers;
using Processes.Services;
using Processes.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Processes.Presenters
{
    class MainPresenter
    {
        private CancellationTokenSource _ctoken;

        private readonly ITimeManager _timeManager;
        private readonly IProcessInfoService _processInfoService;
        private readonly IProcessDetailService _processDetailService;
        private readonly IMainForm _view;
        private readonly ILoggerFactory _loggerFactory;

        public MainPresenter(ITimeManager timeManager,
            IProcessInfoService processInfoService,
            IProcessDetailService processDetailService,
            IMainForm view,
            ILoggerFactory loggerFactory)
        {
            _timeManager = timeManager;
            _processInfoService = processInfoService;
            _processDetailService = processDetailService;
            _view = view;
            _loggerFactory = loggerFactory;

            _ctoken = new CancellationTokenSource();

            _timeManager.Tick += _timeManager_Tick;
            _view.MainFormLoad += _view_MainFormLoad;
            _view.MainFormFormClosed += _view_MainFormFormClosed;
            _view.AddProcessInfo += _view_AddProcessInfo;
        }

        private void _view_AddProcessInfo(object sender, EventArgs e)
        {
            try
            {
                if (sender is IProcessInfoControl)
                {
                    var processDetailPresenter = new ProcessDetailPresenter(_timeManager, sender as IProcessInfoControl, _loggerFactory);
                }
            }
            catch (Exception ex)
            {
                _loggerFactory.ErrorLogged(ex);
            }
        }

        private void _view_MainFormFormClosed(object sender, EventArgs e)
        {
            try
            {
                _loggerFactory.CloseProgramLogged();

                if (_processInfoService != null)
                {
                    _processInfoService.Dispose();
                }

                if (_timeManager != null)
                {
                    _timeManager.Stop();
                    _timeManager.Dispose();
                }
            }
            catch (Exception ex)
            {
                _loggerFactory.ErrorLogged(ex);
            }
        }

        private void _view_MainFormLoad(object sender, EventArgs e)
        {
            try
            {
                TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

                _processInfoService.LoadProcessesInfo(_ctoken)
                    .ContinueWith(ant =>
                    {
                        _view.AddGetNICLabel(_processInfoService.ProcessInfo);

                        _loggerFactory.RunProgramLogged();
                        AddNewProcess();

                        _timeManager.Start();
                    }, CancellationToken.None,
                       TaskContinuationOptions.None,
                       scheduler);
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

                var removeProcess = _processDetailService.GetRemoveProcess();
                if (removeProcess != null && removeProcess.Length > 0)
                {
                    _loggerFactory.RemovedProcessesLogged(removeProcess);
                    _view.RemoveProcess(removeProcess);
                }

                AddNewProcess();

                _processInfoService.LoadProcessesInfo(_ctoken);
            }
            catch (Exception ex)
            {
                _loggerFactory.ErrorLogged(ex);
            }
        }

        private void AddNewProcess()
        {
            try
            {
                var newProcess = _processDetailService.GetNewProcesses();
                if (newProcess != null && newProcess.Length > 0)
                {
                    _loggerFactory.AddedProcessesLogged(newProcess);
                    _view.AddNewProcess(newProcess);
                }
            }
            catch (Exception ex)
            {
                _loggerFactory.ErrorLogged(ex);
            }
        }
    }
}
