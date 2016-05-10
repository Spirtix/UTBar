using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UTBar
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void cbNames_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNames.Checked == true)
            {
                Form1.DisplayNames = true;
                Form1.ChangeConfiguration();
            }
            else
            {
                Form1.DisplayNames = false;
                Form1.ChangeConfiguration();
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (Form1.DisplayNames == true)
            {
                cbNames.Checked = true;
            }
            else
            {
                cbNames.Checked = false;
            }
        }
    }
}
