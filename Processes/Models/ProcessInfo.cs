using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processes.Models
{
    public class ProcessInfo
    {
        public int ID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessCPU { get; set; }
        public string ProcessRAM { get; set; }
        public string ProcessPage { get; set; }
        public IList<ProcessNic> ProcessNics { get; set; }
    }
}
