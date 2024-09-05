using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace code
{
    public partial class LoadingBar : Form
    {
        public LoadingBar()
        {
            InitializeComponent();
        }

        public void Start()
        {
            backgroundWorker1.ProgressChanged += (sender, e) => progressBar1.Value = e.ProgressPercentage;
            backgroundWorker1.RunWorkerCompleted += (sender, e) => { Close(); };

            backgroundWorker1.RunWorkerAsync();
            Show();
        }

        public void Stop()
        {
            if (this.Visible)
                Close();
        }



        private void LoadingBar_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(35);
                backgroundWorker1.ReportProgress(i);
            }
        }
    }
}
