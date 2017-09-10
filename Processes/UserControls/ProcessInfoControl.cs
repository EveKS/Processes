using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Processes.Models;

namespace Processes.UserControls
{
    public partial class ProcessInfoControl : UserControl, IProcessInfoControl
    {
        #region head
        private bool toggle;

        private ProcessDetails _processDetails;
        #endregion

        public ProcessInfoControl()
        {
            InitializeComponent();

            this.Size = new Size(250, 25);
            sSStatusBar.Visible = false;

            bProcessName.Click += ProcessInfoControl_Click;
            this.HandleDestroyed += ProcessInfoControl_HandleDestroyed;
        }

        #region IProcessInfoControl
        public ProcessDetails GetProcessDetails
        {
            get
            {
                return _processDetails;
            }
        }

        public ProcessInfo ProcessInfo
        {
            set
            {
                var settextCPU = new Action(() => { tSCPU.Text = value.ProcessCPU; });
                var settextCPUUserTime = new Action(() => { tSCPUUserTime.Text = value.ProcessCPUUserTime; });
                var settextRAM = new Action(() => { tSRAM.Text = value.ProcessRAM; });
                var settextPage = new Action(() => { tSPage.Text = value.ProcessPage; });

                if (sSStatusBar.InvokeRequired)
                {
                    sSStatusBar.Invoke(settextCPU);
                    sSStatusBar.Invoke(settextCPUUserTime);
                    sSStatusBar.Invoke(settextRAM);
                    sSStatusBar.Invoke(settextPage);
                }
                else
                {
                    settextCPU();
                    settextCPUUserTime();
                    settextRAM();
                    settextPage();
                }
            }
        }

        public event EventHandler ProcessInfoControlClick;
        public event EventHandler ProcessInfoControlHandleDestroyed;
        #endregion

        #region props
        public int ControlWidth
        {
            set
            {
                var settextAction = new Action(() => { this.Width = value; });

                if (this.InvokeRequired)
                    this.Invoke(settextAction);
                else
                    settextAction();
            }
        }

        public ProcessDetails ProcessName
        {
            set
            {
                _processDetails = value;
                var settextAction = new Action(() =>
                {
                    bProcessName.Text = value.ProcessName;
                });

                if (bProcessName.InvokeRequired)
                    bProcessName.Invoke(settextAction);
                else
                    settextAction();
            }
        }

        public ProcessDetails ToolTip
        {
            set
            {
                var settextAction = new Action(() =>
                {
                    var tTip = string.Format("ID:\t{0}\nName:\t{1}", value.ID, value.ProcessName);

                    toolTip.SetToolTip(this.bProcessName, tTip);
                });

                if (bProcessName.InvokeRequired)
                    bProcessName.Invoke(settextAction);
                else
                    settextAction();
            }
        }
        #endregion

        #region ections
        private void ProcessInfoControl_HandleDestroyed(object sender, EventArgs e)
        {
            if (ProcessInfoControlHandleDestroyed != null)
            {
                ProcessInfoControlHandleDestroyed.Invoke(this, EventArgs.Empty);
            }
        }

        private void ProcessInfoControl_Click(object sender, EventArgs e)
        {
            toggle ^= true;

            if (toggle)
            {
                this.Height = 50;
                sSStatusBar.Visible = true;
            }
            else
            {
                this.Height = 25;
                sSStatusBar.Visible = false;
            }

            if (ProcessInfoControlClick != null)
            {
                ProcessInfoControlClick.Invoke(toggle, EventArgs.Empty);
            }
        }
        #endregion
    }
}
