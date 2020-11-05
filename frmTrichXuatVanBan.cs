using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace MultiFaceRec
{
    public partial class frmTrichXuatVanBan : Form
    {
        public frmTrichXuatVanBan()
        {
            InitializeComponent();
        }

        private void frmTrichXuatVanBan_Load(object sender, EventArgs e)
        {
            btnLuu.Visible = false;
        }

        private void frmTrichXuatVanBan_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            new frmOption().Show();
        }

        private void btnTaiDuLieu_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF files (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder content = new StringBuilder();
                try
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                    btnLuu.Visible = true;

                    using (PdfReader reader = new PdfReader(dialog.FileName))
                    {
                        ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();


                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            content.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                        }
                    }

                }
                catch { }

                txtKetqua.Text = content.ToString();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, txtKetqua.Text);
                MessageBox.Show("Đã lưu file ở " + saveFileDialog1.FileName);
            }
        }
    }
}
