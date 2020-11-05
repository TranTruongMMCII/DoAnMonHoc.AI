using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Spire.Pdf;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace MultiFaceRec
{
    public partial class frmTrichXuatHinhAnh : Form
    {
        public frmTrichXuatHinhAnh()
        {
            InitializeComponent();
        }
        List<Image> ListImage;
        int index = 0;
        private void btnTaiDuLieu_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF files (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                    btnLuu.Visible = true;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;

                    PdfDocument doc = new PdfDocument();
                    doc.LoadFromFile(dialog.FileName);
                    ListImage = new List<Image>();
                    for (int i = 0; i < doc.Pages.Count; i++)
                    {
                        // Get an object of Spire.Pdf.PdfPageBase
                        PdfPageBase page = doc.Pages[i];
                        // Extract images from Spire.Pdf.PdfPageBase
                        Image[] images = page.ExtractImages();
                        if (images != null && images.Length > 0)
                        {
                            ListImage.AddRange(images);
                        }

                    }

                    pictureBox1.Image = new Bitmap(ListImage[0]);

                }
                catch { }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            new frmOption().Show();
        }

        private void frmTrichXuatHinhAnh_Load(object sender, EventArgs e)
        {
            btnLuu.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ListImage.Count > 0)
            {
                if (index - 1 < 0)
                {
                    index = ListImage.Count;
                }
                index--;
                pictureBox1.Image = new Bitmap(ListImage[index]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ListImage.Count > 0)
            {
                if (index + 1 >= ListImage.Count)
                {
                    index = -1;
                }
                index++;
                pictureBox1.Image = new Bitmap(ListImage[index]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListImage.RemoveAt(index);
            if (ListImage.Count > 0)
            {
                if (index + 1 >= ListImage.Count)
                {
                    index = 0;
                }

                pictureBox1.Image = new Bitmap(ListImage[index]);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "png files(*.png)|*.png";
            //saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ListImage[index].Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Đã lưu file ở " + saveFileDialog1.FileName);
            }
        }
    }
}
