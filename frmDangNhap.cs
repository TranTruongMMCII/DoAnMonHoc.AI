using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class frmDangNhap : Form
    {
        public static string tendangnhap;
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        List<string> NamePersons = new List<string>();
        List<string> labels = new List<string>();
        int ContTrain, NumLabels, t;
        Image<Gray, byte> gray = null;
        Image<Gray, byte> result, TrainedFace = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();


        HaarCascade face;
        string name, names = null;

        public frmDangNhap()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(name))
            {
                timer1.Enabled = false;
                DialogResult dialogResult = MessageBox.Show("Bạn muốn tiếp tục với " + name + ".", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                if (dialogResult == DialogResult.Yes)
                {
                    //grabber.Dispose();
                    this.Close();
                    new frmOption().Show();

                }
                else
                {
                    this.Hide();
                    new frmMain().Show();
                }

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            new frmMain().Show();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            try
            {
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            grabber = new Capture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber);

            //while (true){

            //}
        }

        void FrameGrabber(object sender, EventArgs e)
        {


            //Get the current frame form capture device
            try
            {
                currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            }
            catch
            {
                return;
            }

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);

                    name = recognizer.Recognize(result);
                    //tendangnhap = name;
                   
                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }
                

            }


            t = 0;
            pictureBox1.Image = currentFrame.ToBitmap();
            Console.WriteLine(tendangnhap);
         
            NamePersons.Clear();
            
        }
    }
}
