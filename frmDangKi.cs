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
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class frmDangKi : Form
    {
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        List<string> NamePersons = new List<string>();
        List<string> labels = new List<string>();
        int ContTrain, NumLabels, t;
        Image<Gray, byte> gray = null;
        Image<Gray, byte> result, TrainedFace = null;


        HaarCascade face;
        string name, names = null;


        public frmDangKi()
        {
            InitializeComponent();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
        }
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();

        private void frmDangKi_Load(object sender, EventArgs e)
        {
            btnLuu.Visible = false;
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
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmMain().Show();
        }
 

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

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
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                picDetected.Image = TrainedFace.ToBitmap();
            }
            catch
            {
                Console.WriteLine("failed!");
            }
            trainingImages.Add(TrainedFace);
            labels.Add(frmTenDangKi.name);

            //Show face added in gray scale


            //Write the number of triained faces in a file text for further load
            string temp = Application.StartupPath + "/TrainedFaces/TrainedLabels.txt";
            File.WriteAllText(temp, trainingImages.ToArray().Length.ToString() + "%");

            //Write the labels of triained faces in a file text for further load
            for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
            {
                trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
            }

            MessageBox.Show(frmTenDangKi.name + "´s face detected and added successfully. Thank you!", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            frmDangNhap frmDangNhap = new frmDangNhap();
            frmDangNhap.Show();
        }


        private void btnDangKy_Click(object sender, EventArgs e)
        {
            frmTenDangKi frmTenDangKi = new frmTenDangKi();
            frmTenDangKi.ShowDialog();
            btnLuu.Visible = true;
            btnDangKy.Visible = false;
            grabber = new Capture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber);

            btnDangKy.Enabled = false;


            
        }

        void FrameGrabber(object sender, EventArgs e)
        {


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

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

                    //name = recognizer.Recognize(result);

                    //Draw the label for each face detected and recognized
                    //currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }
                

                //NamePersons[t - 1] = name;
                //NamePersons.Add("");


            }

            
            t = 0;

       
            picCapture.Image = currentFrame.ToBitmap();

            NamePersons.Clear();

        }



    }
}

