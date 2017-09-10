using Processes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Processes.Services
{
    class LoggerFactory : ILoggerFactory
    {
        private readonly string path;
        private CancellationTokenSource _ctoken;

        public LoggerFactory()
        {
            _ctoken = new CancellationTokenSource();
            path = Application.StartupPath + "\\loggs.txt";
        }

        #region ILoggerFactory
        void ILoggerFactory.AddedProcessesLogged(params ProcessDetails[] processDetails)
        {
            var appendInfo = string.Format("{0:dd.MM.yy hh:mm:ss}\tappend new processes", DateTime.Now);
            string[] process = GetProcessDetailsStrings(processDetails);

            WriteInfo(appendInfo, process);
        }

        void ILoggerFactory.RemovedProcessesLogged(params ProcessDetails[] processDetails)
        {
            var appendInfo = string.Format("{0:dd.MM.yy hh:mm:ss}\tremoved processes", DateTime.Now);
            string[] process = GetProcessDetailsStrings(processDetails);

            WriteInfo(appendInfo, process);
        }

        void ILoggerFactory.UserOpenInfoLogged(ProcessDetails processDetails)
        {
            var appendInfo = string.Format("{0:dd.MM.yy hh:mm:ss}\tuser open", DateTime.Now);
            string[] process = GetProcessDetailsStrings(processDetails);

            WriteInfo(appendInfo, process);
        }

        void ILoggerFactory.UserCloseInfoLogged(ProcessDetails processDetails)
        {
            var appendInfo = string.Format("{0:dd.MM.yy hh:mm:ss}\tuser close", DateTime.Now);
            string[] process = GetProcessDetailsStrings(processDetails);

            WriteInfo(appendInfo, process);
        }

        void ILoggerFactory.RunProgramLogged()
        {
            var appendInfo = string.Format("{0:dd.MM.yy hh:mm:ss}\tprogram run", DateTime.Now);
            WriteInfo(appendInfo);
        }

        void ILoggerFactory.CloseProgramLogged()
        {
            var appendInfo = string.Format("{0:dd.MM.yy hh:mm:ss}\tprogram close", DateTime.Now);
            WriteInfo(appendInfo);
        }

        void ILoggerFactory.ErrorLogged(Exception ex)
        {
            var appendInfo = string.Format("{0:dd.MM.yy hh:mm:ss}\texception", DateTime.Now);

            string[] exDetail =
            {
                string.Format("Member name:\t{0}", ex.TargetSite),
                string.Format("Class defining member:\t{0}", ex.TargetSite.DeclaringType),
                string.Format("Member Type:\t{0}", ex.TargetSite.MemberType),
                string.Format("Message:\t{0}", ex.Message),
                string.Format("Source:\t{0}", ex.Source),
                string.Format("Help Link:\t{0}", ex.HelpLink),
                string.Format("Stack:\t{0}", ex.StackTrace),
            };

            WriteInfo(appendInfo, exDetail);
        }
        #endregion

        private string[] GetProcessDetailsStrings(params ProcessDetails[] processDetails)
        {
            var length = processDetails.Length;

            string[] process = new string[length];
            for (int i = 0; i < length; i++)
            {
                process[i] = string.Format("{0:dd.MM.yy hh:mm:ss} process:\t{1} ID:\t{2}",
                    DateTime.Now, processDetails[i].ProcessName, processDetails[i].ID);
            }

            return process;
        }

        private void WriteInfo(string info)
        {
            WriteInfo(info, null);
        }

        private Task WriteInfo(string info, params string[] details)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!string.IsNullOrEmpty(info) && !string.IsNullOrEmpty(path))
                {
                    using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
                    {
                        streamWriter.WriteLine(info);

                        if (details != null)
                        {
                            for (int i = 0; i < details.Length; i++)
                            {
                                streamWriter.WriteLine(details[i]);
                            }
                        }
                    }
                }
            },
            _ctoken.Token);
        }
    }
}
