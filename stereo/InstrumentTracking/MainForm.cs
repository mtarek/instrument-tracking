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
    public partial class MainForm : Form
    {
        private ThorMotorController motorX;
        private ThorMotorController motorY;
        private ThorMotorController motorZ;

        private Camera left;
        private Camera right;

        private CalibrationModel model;
        private CalibrationForm calibForm;

        bool stageInitialized;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            left = new Camera();
            right = new Camera();
            stageInitialized = false;
            UpdateButtonStates();
            CameraForm cfrm = new CameraForm(left, right);
            cfrm.MdiParent = this;
            cfrm.Show();
            model = new CalibrationModel(17.5);
            calibForm = new CalibrationForm(left, right, model);
            calibForm.MdiParent = this;
            calibForm.Show();
            
        }

        private void UpdateButtonStates()
        {
                btnCalibrateCameras.Enabled = stageInitialized;
                btnHomeStage.Enabled = stageInitialized;
                btnInitializeStage.Enabled = !stageInitialized;
        }

        private void btnInitializeStage_Click(object sender, EventArgs e)
        {
            InitializeStage();
        }

        private void  InitializeStage()
        {
            if (!stageInitialized)
            {
                motorX = new ThorMotorController(83847540);
                motorY = new ThorMotorController(83847484);
                motorZ = new ThorMotorController(83848986);
                motorX.MdiParent = this;
                motorY.MdiParent = this;
                motorZ.MdiParent = this;

                motorX.Show();
                motorY.Show();
                motorZ.Show();

                model.SetMotor(0, motorX);
                model.SetMotor(1, motorY);
                model.SetMotor(2, motorZ);

                stageInitialized = true;
            }

            UpdateButtonStates();

        }

        private void btnCalibrateCameras_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < 5; i++)
            {
                model.Move(0.1, 0.1, 0.1);
            }
        }

        private void btnHomeStage_Click(object sender, EventArgs e)
        {
            motorX.MoveHome(false);
            motorY.MoveHome(false);
            motorZ.MoveHome(false);
        }

    }
}
