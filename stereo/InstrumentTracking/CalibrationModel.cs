using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentTracking
{
    /* Currently only assumes a square model with 4 points and a known side length */

    public class CalibrationModel
    {
        private Point3D[] points; // 'Origin' is taken as the top left corner, and points are sorted clockwise
        private double sideLength;
        private IMotorController[] motor; // handles to moving stage motor controllers 0:X, 1:Y, 2:Z
        private const int MAX_POINTS = 4;

        public event EventHandler Moved;

        public CalibrationModel(double sideLength)
        {
            points = new Point3D[4];
            for (int i = 0; i < MAX_POINTS; i++)
            {
                points[i] = new Point3D();
            }
            points[0].Offset(0, 0, 0);
            points[1].Offset(sideLength, 0, 0);
            points[2].Offset(sideLength, sideLength, 0);
            points[3].Offset(0, sideLength, 0);

            motor = new IMotorController[3];
            
        }

        public void SetMotor(int axis, IMotorController motorController)
        {
            if (axis > 2) throw new ArgumentOutOfRangeException("axis");
            motor[axis] = motorController;
        }

        public void Move(double dispX, double dispY, double dispZ)
        {
            motor[0].MoveRelative((float)dispX, false);
            motor[1].MoveRelative((float)dispY, false);
            motor[2].MoveRelative((float)dispZ, true);
            foreach (Point3D point in points)
            {
                point.Offset(dispX, dispY, dispZ);
            }

            /* Raise notification that the stage moved */
            if (Moved != null) Moved(this, null);
        }

        public override string ToString()
        {
            string s = "";

            for (int i = 0; i < MAX_POINTS; i++)
            {
                s += i + ": " + points[i].ToString() + "\n";
            }
            s += "\n";
            return s;
        }

        public Point3D GetPoint(int index)
        {
            if (index > points.Length) throw new IndexOutOfRangeException();

            return points[index];
        }

        public static void SortPointsToModel(out Point[] point)
        {
            throw new NotImplementedException();
        }


    }
}
