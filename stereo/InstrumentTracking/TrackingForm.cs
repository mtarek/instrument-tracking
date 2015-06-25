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
    public partial class TrackingForm : Form
    {
        public TrackingForm()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            Graphics g = pbTracking.CreateGraphics();
            g.Clear(Color.Black);
        }

        public int Width
        {
            get{ return pbTracking.Width;}
        }

        public int Height
        {
            get{return pbTracking.Height;}
        }

        public void Draw(int x, int y, double angle)
        {
            Graphics g = pbTracking.CreateGraphics();
            g.DrawEllipse(new Pen(Color.White), 4 * x + this.Width / 2, 4 * y + this.Height / 2, 1, 1);
            label1.Text = "X = " + x + "\nY = " + y + "\na= " + angle;
        }

        public void DrawInstrument(double x1, double y1, double x2, double y2, double x3, double y3, double angle = 0)
        {
            Graphics g = pbTracking.CreateGraphics();
            g.Clear(Color.Black);
            int offx = Width / 2;
            int offy = Height / 2;
            g.DrawEllipse(new Pen(Color.White), (int)x1 + offx, (int)y1+offy, 3, 3);
            g.DrawEllipse(new Pen(Color.White), (int)x2 + offx, (int)y2 + offy, 3, 3);
            g.DrawEllipse(new Pen(Color.White), (int)x3 + offx, (int)y3 + offy, 3, 3);
            label1.Text = "X = " + x3 + "\nY = " + y3 + "\na= " + angle;
        }

        public void DrawLine(int len)
        {
            Graphics g = pbTracking.CreateGraphics();
            g.Clear(Color.Black);
            g.DrawLine(new Pen(Color.White), new PointF(Width / 2, Height / 2 - len / 2), new PointF(Width / 2, Height / 2 + len / 2));
        }
    }
}
