using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processes.Utilitys;
using Processes.Models;

namespace ProcessesTests
{
    [TestClass]
    public class ProcessesInfosUtilityTest
    {
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
    }
}
