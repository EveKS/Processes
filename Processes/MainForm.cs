using Processes.Models;
using Processes.UserControls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Processes
{
    public partial class MainForm : Form, IMainForm
    {
        #region Head
        private const int LABEL_WIDTH = 60;
        #endregion

        public MainForm()
        {
            InitializeComponent();
            SetAppName();

            this.Load += MainForm_Load;
            this.FormClosed += MainForm_FormClosed;
        }

        #region IMain
        public event EventHandler MainFormLoad;
        public event EventHandler MainFormFormClosed;
        public event EventHandler AddProcessInfo;

        public ProcessInfo ProcessInfo
        {
            set
            {
                var settextCPU = new Action(() => { tSCPU.Text = value.ProcessCPU; });
                var settextRAM = new Action(() => { tSRAM.Text = value.ProcessRAM; });
                var settextPage = new Action(() => { tSPage.Text = value.ProcessPage; });

                if (sSStatusBar.InvokeRequired)
                {
                    sSStatusBar.Invoke(settextCPU);
                    sSStatusBar.Invoke(settextRAM);
                    sSStatusBar.Invoke(settextPage);
                }
                else
                {
                    settextCPU();
                    settextRAM();
                    settextPage();
                }

                for (int i = 0; i < value.ProcessNics.Count; i++)
                {
                    var settextAction = new Action(() => { sSStatusBar.Items[String.Format("tSNIC{0}", i)].Text = value.ProcessNics[i].Value; });
                    if (sSStatusBar.InvokeRequired)
                    {
                        sSStatusBar.Invoke(settextAction);
                    }
                    else
                    {
                        settextAction();
                    }
                }
            }
        }

        public void AddNewProcess(ProcessDetails[] processDetails)
        {
            for (var i = 0; i < processDetails.Length; i++)
            {
                var processInfo = new ProcessInfoControl
                {
                    ProcessName = processDetails[i],
                    ToolTip = processDetails[i],
                    ControlWidth = fLPMainPanel.Width - 25,
                    Name = string.Format("{0}{1}", processDetails[i].ProcessName, processDetails[i].ID),
                    Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right)
                };

                AddInfo(processInfo);
                if (AddProcessInfo != null)
                {
                    AddProcessInfo.Invoke(processInfo, EventArgs.Empty);
                }
            }
        }

        public void RemoveProcess(ProcessDetails[] processDetails)
        {
            var removeNames = processDetails.Select(info => string.Format("{0}{1}", info.ProcessName, info.ID));

            var removeControls = fLPMainPanel.Controls.OfType<ProcessInfoControl>()
                .Where(c => removeNames.Contains(c.Name));

            foreach (var control in removeControls)
            {
                RemoveInfos(control);
            }
        }

        public void AddGetNICLabel(ProcessInfo processInfo)
        {
            var count = processInfo.ProcessNics.Count;
            for (int i = 0; i < count; i++)
            {
                sSStatusBar.Items.Add(GetNICLabel(processInfo.ProcessNics[i].Name, i));
            }
        }
        #endregion

        #region events
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainFormFormClosed != null)
            {
                MainFormFormClosed.Invoke(this, EventArgs.Empty);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (MainFormLoad != null)
            {
                MainFormLoad.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Services
        private void SetAppName()
        {
            this.Text = String.Format("{0} [{1}]", Application.ProductName, Environment.MachineName);
        }

        private void RemoveInfos(ProcessInfoControl removeControl)
        {
            var settextAction = new Action(() => { fLPMainPanel.Controls.Remove(removeControl); });

            if (fLPMainPanel.InvokeRequired)
                fLPMainPanel.Invoke(settextAction);
            else
                settextAction();
        }

        private void AddInfo(ProcessInfoControl processInfo)
        {
            var settextAction = new Action(() => { fLPMainPanel.Controls.Add(processInfo); });

            if (fLPMainPanel.InvokeRequired)
                fLPMainPanel.Invoke(settextAction);
            else
                settextAction();
        }

        private ToolStripStatusLabel GetNICLabel(string instanceName, int index)
        {
            ToolStripStatusLabel newLabel = new ToolStripStatusLabel
            {
                AutoSize = false,
                Width = LABEL_WIDTH,
                ToolTipText = instanceName,
                Text = string.Empty,
                Name = String.Format("tSNIC{0}", index),
                TextAlign = ContentAlignment.MiddleRight,
                BorderSides = ToolStripStatusLabelBorderSides.All,
                BorderStyle = Border3DStyle.SunkenInner,
                Height = sSStatusBar.Items[0].Height
            };

            return newLabel;
        }
        #endregion
    }
}
