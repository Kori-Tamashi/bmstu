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
    public partial class ErrorMessage : Form
    {
        public ErrorMessage()
        {
            InitializeComponent();
        }

        private void ErrorMessage_Load(object sender, EventArgs e)
        {

        }

        public void SetText(string text)
        {
            richTextBox1.Text = text;
        }

        public void Show(string text)
        {
            SetText(text);
            ShowDialog();
        }
    }
}
