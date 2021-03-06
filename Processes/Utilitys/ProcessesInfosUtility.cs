﻿using Processes.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Processes.Utilitys
{
    public class ProcessesInfosUtility : IProcessesInfosUtility
    {
        private ProcessDetails[] _processDetails;

        public void LoadProcessesNames()
        {
            IEnumerable<Process> procList = Process.GetProcesses()
                .OrderBy(p => p.ProcessName).Where(p => p.ProcessName != "Idle");

            _processDetails = procList.Select(process =>
            {
                using (process)
                {
                    return new ProcessDetails
                    {
                        ID = process.Id,
                        ProcessName = process.ProcessName,
                        PrivateMemorySize64 = process.PrivateMemorySize64
                    };
                }
            })
            .ToArray();
        }

        int IProcessesInfosUtility.Length
        {
            get { return _processDetails.Length; }
        }

        ProcessDetails IProcessesInfosUtility.this[int index]
        {
            get
            {
                if (_processDetails != null && index >= 0
                    && index < _processDetails.Length)
                {
                    return _processDetails[index];
                }

                return null;
            }
        }
    }
}
