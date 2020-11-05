using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class frmTenDangKi : Form
    {
        public static string name = "";
        public frmTenDangKi()
        {
            InitializeComponent();
        }

        private void frmTenDangKi_Load(object sender, EventArgs e)
        {
            txtTenDangKi.Focus();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //ToolTip myToolTip = new ToolTip();
            //myToolTip.SetToolTip(txtTenDangKi, "Mời bạn nhập vào tên của bạn: ");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtTenDangKi.Text != "")
            {
                name = txtTenDangKi.Text;
                this.Close();
            }
            else { }
        }

        private void txtTenDangKi_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenDangKi_Enter(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
        }

        private void txtTenDangKi_Enter_1(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
        }
    }
}
