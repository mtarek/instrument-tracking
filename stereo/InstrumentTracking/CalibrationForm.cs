using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstrumentTracking
{
    public partial class CalibrationForm : Form
    {
        private Camera left;
        private Camera right;
        private CalibrationModel model;

        public CalibrationForm(Camera left, Camera right, CalibrationModel model)
        {
            InitializeComponent();

            this.left = left;
            this.right = right;
            this.model = model;

            model.Moved += model_Moved;
        }

        private void model_Moved(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                LeftLogLine(GetCalibrationPointString(left, model, i));
                RightLogLine(GetCalibrationPointString(right, model, i));
            }
        }

        public void LeftLogLine(string line)
        {
            tbLeft.Text += line + "\r\n";
        }

        public void RightLogLine(string line)
        {
            tbRight.Text += line + "\r\n";
        }

        /* Form on line of calibration data that represents one calibration point in the format 
         * expected by Tsai3D calibration program */
        private string GetCalibrationPointString(Camera cam, CalibrationModel model, int pointIndex)
        {
            string res =
                model.GetPoint(pointIndex).X + " " + model.GetPoint(pointIndex).Y + " " + model.GetPoint(pointIndex).Z + " " + cam.points[pointIndex].X + " " + cam.points[pointIndex].Y;
            return res;
        }
    }
}
