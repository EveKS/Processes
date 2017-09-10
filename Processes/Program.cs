using Processes.Managers;
using Processes.Presenters;
using Processes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Processes
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm form = new MainForm();
            ITimeManager timeManager = new TimeManager();
            IProcessDetailService processDetailService = new ProcessDetailService();
            IProcessInfoService processInfoService = new ProcessInfoService();
            ILoggerFactory loggerFactory = new LoggerFactory();

            MainPresenter presenter = new MainPresenter(timeManager,
                processInfoService,
                processDetailService,
                form,
                loggerFactory);

            Application.Run(form);
        }
    }
}
