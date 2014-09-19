using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WiimoteLib;

namespace InstrumentTracking
{
    struct Intrinsic
    {

    }

    struct Extrinsic
    {

    }

    public class Camera
    {
        /* Wiimote-specific declration */
        private static int numWiimotes = 0; // Keep track of the number of connected wiimotes
        private Wiimote wm;
        /* End wiimote specific declarations */

        private const int MAX_POINTS = 4;
        private Intrinsic A;
        private Extrinsic RT;

        public Point2D[] points;

        public Camera()
        {
            wm = new Wiimote();
            wm.Connect(++numWiimotes);
            wm.SetReportType(InputReport.IRAccel, true);
            wm.SetLEDs(numWiimotes);
            wm.WiimoteChanged += wm_WiimoteChanged;

            points = new Point2D[MAX_POINTS];
            for (int i = 0; i < MAX_POINTS; i++)
            {
                points[i] = new Point2D();
            }

        }

        public override string ToString()
        {
            string s = "";

            for(int i = 0; i < MAX_POINTS; i++)
            {
                s += i+ ": " + points[i].ToString() + "\n" ;
            }
            s += "\n";
            return s;
        }

        private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            UpdateWiimoteState(e);
        }

        private void UpdateWiimoteState(WiimoteChangedEventArgs args)
        {
            WiimoteState ws = args.WiimoteState;

            for (int j = 0; j < 4; j++)
            {
                if (ws.IRState.IRSensors[j].Found)
                {
                    points[j] = ws.IRState.IRSensors[j].RawPosition;
                    //TODO: Sort points?
                }

            }
        }

        ~Camera()
        {
            wm.SetLEDs(0); // Turn off LEDs
            wm.Disconnect();
        }

    }
}
