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
    public partial class ThorMotorController : Form, IMotorController
    {
        public ThorMotorController(int serialNumber)
        {
            InitializeComponent();

            axMG17Motor1.HWSerialNum = serialNumber;
            axMG17Motor1.StartCtrl();
        }

        public void MoveHome(bool waitTillDone)
        {
            axMG17Motor1.MoveHome(0, waitTillDone);
        }

        public void MoveAbsolute(float position, bool waitTillDone)
        {
            axMG17Motor1.SetAbsMovePos(0, position);
            axMG17Motor1.MoveAbsolute(0, waitTillDone);
        }

        public void MoveRelative(float displacement, bool waitTillDone)
        {
            axMG17Motor1.SetRelMoveDist(0, displacement);
            axMG17Motor1.MoveRelative(0, waitTillDone);  
        }

        public void Display(object data)
        {
            this.ShowDialog();
        }


        public float GetPosition()
        {
            float pos = 0;
            axMG17Motor1.GetPosition(0, ref pos);
            return pos;
        }
    }
}
