using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processes.Utilitys;
using Processes.Services;
using Processes.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace ProcessesTests
{
    [TestClass]
    public class UtilityTests
    {
        #region ProcessesInfosUtility
        [TestMethod]
        public void ProcessesInfosUtility()
        {
            IProcessesInfosUtility processesInfosUtility = new ProcessesInfosUtility();
            processesInfosUtility.LoadProcessesNames();

            Assert.IsNotNull(processesInfosUtility);
            Assert.IsInstanceOfType(processesInfosUtility[0], typeof(ProcessDetails));
        }

        [TestMethod]
        public void ProcessesInfosUtilityLength()
        {
            IProcessesInfosUtility processesInfosUtility = new ProcessesInfosUtility();
            processesInfosUtility.LoadProcessesNames();

            Assert.IsInstanceOfType(processesInfosUtility.Length, typeof(int));
        }
        #endregion

        #region ProcessInfoService
        [TestMethod()]
        public void ProcessInfoServiceDispose()
        {
            using (IProcessInfoService processInfoService = new ProcessInfoService())
            {
                Assert.IsInstanceOfType(processInfoService, typeof(IDisposable));
            }
        }

        [TestMethod()]
        public void ProcessInfoServiceNotNull()
        {
            using (IProcessInfoService processInfoService = new ProcessInfoService())
            {
                processInfoService.LoadProcessesInfo().Wait();
                Assert.IsNotNull(processInfoService.ProcessInfo);
            }
        }
        #endregion

        #region ProcessInfoService with Params
        private string GetProcess()
        {
            return Process.GetProcesses().FirstOrDefault().ProcessName;
        }

        [TestMethod()]
        public void ProcessInfoServiceWithParamDispose()
        {
            var process = GetProcess();

            using (IProcessInfoService processInfoService = new ProcessInfoService(process))
            {
                Assert.IsInstanceOfType(processInfoService, typeof(IDisposable));
            }
        }

        [TestMethod()]
        public void ProcessInfoServiceWithParamNotNull()
        {
            var process = GetProcess();

            using (IProcessInfoService processInfoService = new ProcessInfoService(process))
            {
                processInfoService.LoadProcessesInfo().Wait();
                Assert.IsNotNull(processInfoService.ProcessInfo);
            }
        }
        #endregion

        #region ProcessDetailService
        [TestMethod()]
        public void ProcessDetailServiceGetNewProcessesNotNull()
        {
            IProcessDetailService processDetailService = new ProcessDetailService();
            var newProcess = processDetailService.GetNewProcesses();

            Assert.IsNotNull(newProcess);
            Assert.IsInstanceOfType(newProcess, typeof(ProcessDetails[]));
        }

        [TestMethod()]
        public void ProcessDetailServiceGetRemoveProcessIsNullOrProcessDetailsArray()
        {
            IProcessDetailService processDetailService = new ProcessDetailService();
            var removeProcess = processDetailService.GetRemoveProcess();

            Assert.IsTrue(removeProcess == null || typeof(ProcessDetails[]) == removeProcess.GetType());
        }
        #endregion
    }
}
