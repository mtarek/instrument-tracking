using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstrumentTracking
{
    public partial class CameraForm : Form
    {
        private Camera left;
        private Camera right;

        CalibrationModel model;

        public CameraForm(Camera left, Camera right)
        {
            InitializeComponent();

            this.left = left;
            this.right = right;

            timerPbRefresh.Start();
        }

        private void timerPbRefresh_Tick(object sender, EventArgs e)
        {
            UpdateCamDisplay();
        }

        private void UpdateCamDisplay()
        {
            //TODO: REFACTOR
            float xScale = pictureBoxLeft.Width / 1024f;
            float yScale = pictureBoxLeft.Height / 768f;
            Graphics gLeft = pictureBoxLeft.CreateGraphics();
            Graphics gRight = pictureBoxRight.CreateGraphics();
            gLeft.Clear(Color.Black);
            gRight.Clear(Color.Black);
            for (int i = 0; i < left.points.Length; i++)
            {
                gLeft.DrawEllipse(new Pen(Color.White), (int)left.points[i].X * xScale, (int)left.points[i].Y * yScale, 3, 3);
                gRight.DrawEllipse(new Pen(Color.White), (int)right.points[i].X * xScale , (int)right.points[i].Y * yScale, 3,3);
            }
            gLeft.DrawString(left.ToString(), new Font("Arial", 32*xScale), Brushes.Green, 0, 0);
            gRight.DrawString(right.ToString(), new Font("Arial", 32*xScale), Brushes.Green, 0, 0);
        }


    }
}
