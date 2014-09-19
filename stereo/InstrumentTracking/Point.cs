using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentTracking
{
    public class Point
    {
    
    }

    public class Point2D : Point
    {
        public double X;
        public double Y;

        public Point2D()
        {
            X = 0;
            Y = 0;
        }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }


        public static implicit operator Point2D(System.Drawing.Point p)
        {
            return new Point2D(p.X, p.Y);
        }

        public void Offset(double offX, double offY)
        {
            X += offX;
            Y += offY;
        }

        public override string ToString()
        {
            return  "(" + X + "," + Y + ")";
        }
    }

    public class Point3D : Point2D
    {
        public double Z;

        
        public Point3D():base()
        {
            Z = 0;
        }

        public void Offset(double offX, double offY, double offZ)
        {
            X += offX;
            Y += offY;
            Z += offZ;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }
    }

}
