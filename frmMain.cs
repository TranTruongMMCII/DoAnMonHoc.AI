using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDangKi frmDangKi = new frmDangKi();
            frmDangKi.Show();  
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmDangNhap().Show();
        }
    }
}
