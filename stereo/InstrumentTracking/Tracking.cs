using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentTracking
{
    class Tracking
    {
        /* TODO: MOVE THOR STAGE CONTROL TO MODEL.CS */
        private ThorController modelControlX;
        private ThorController modelControlY;
        private ThorController modelControlZ;
        private Camera[] camera;

        public Tracking(string serialNumberX, string serialNumberY, string serialNumberZ)
        {
            //modelControlX = new ThorController(serialNumberX);
            //modelControlY = new ThorController(serialNumberY);
            //modelControlZ = new ThorController(serialNumberZ);
        }

        /* Assumes square model with 4 points */
        void Calibrate(int numAcquisitionsPerView, int numViews, double maxDisplacementX, double maxDisplacementY, double maxDisplacementZ)
        {
            CalibrationModel model = new CalibrationModel(17.5);
            double stepX = (maxDisplacementX / numAcquisitionsPerView);
            double stepY = (maxDisplacementY / numAcquisitionsPerView);
            double stepZ = (maxDisplacementZ / numViews);

            CalibrationView[] views = new CalibrationView[numViews];
            int viewIdx = 0;

            for (double z = 0; z < maxDisplacementZ; z += stepZ, viewIdx++)
            {
                for (double y = 0; y < maxDisplacementY; y += stepY)
                {
                    for (double x = 0; x < maxDisplacementX; x += stepX)
                    {
                        modelControlX.Move(x);
                        modelControlY.Move(y);
                        modelControlZ.Move(z);
                        model.Move(x,y,z);

                        for(int i = 0; i < 4; i++)
                        {
                            views[viewIdx].AddPoint(camera[0].points[i], camera[1].points[i], model.GetPoint(i));
                        }
                    }
                }
            }

            Stereo.CalibrateMultipleViews(views);
        }

    }
}
