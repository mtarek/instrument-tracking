using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentTracking
{
    class Stereo
    {        
        public static int CalibrateSingleView(CalibrationView view)
        {
            throw new NotImplementedException();
        }

        public static int CalibrateMultipleViews(CalibrationView[] views)
        {
            int res;
            foreach (CalibrationView view in views)
            {
                res = CalibrateSingleView(view);

                if (res < 0) return res;
            }

            return 0;
        }


        public static Point3D TriangulatePoint(Point2D image1, Point2D image2)
        {
            throw new NotImplementedException();
        }
    }
}
