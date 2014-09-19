using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentTracking
{   
    /* Data for a single calibration point */
    class CalibrationPoint
    {
        private Point2D image1; // Image from camera 1
        private Point2D image2; // Image from camera 2
        private Point3D worldCoords; // Actual 3D coordinates of point

        public CalibrationPoint(Point2D img1, Point2D img2, Point3D actualCoords)
        {
            image1 = img1;
            image2 = img2;
            worldCoords = actualCoords;
        }
    }

    /* Holds data from a single calibration view */
    class CalibrationView
    {
        private List<CalibrationPoint> points;

        public void AddPoint(Point2D pointImg1, Point2D pointImg2, Point3D actualCoords)
        {
            points.Add(new CalibrationPoint(pointImg1, pointImg2, actualCoords));
        }


    }
}
