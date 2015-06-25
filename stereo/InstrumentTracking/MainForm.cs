using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using NationalInstruments;
using NationalInstruments.DAQmx;
using FFTWSharp;
using System.Runtime.InteropServices;
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

        private bool stageInitialized;

        private bool isTracking;
        private int trackingStarted;
        private bool logTrackingData;
        private Vector3 origin;
        private TrackingForm trackForm;
        private Vector3 top, mid, bot;
        private double ln, lp, q, l1, l2;
        private double instrumentAngle;

        private System.IO.StreamWriter file;

        private NationalInstruments.DAQmx.Task daqTask;
        private NationalInstruments.DAQmx.Task daqTask2;
        private NationalInstruments.DAQmx.Task daqTask3;
        private AnalogMultiChannelReader reader;
        private AnalogMultiChannelWriter writer, writer2;
        private AnalogWaveform<double>[] inDriveSignal;
        private System.AsyncCallback daqCallback;
        private int sampleIndex = 0, sampleIndex2 = 0;
        private int phase = 52;
        private int sampleRate = 100000;
        private int bufsize = 0;
        private double measuredFreq1, measuredFreq2;
        private int samplePerChannel = 7500 / (15);
        private double[][] waveX;
        private double[][] waveY;
        private double[,] outWave;
        private double dcOffsetX, prevDcX = 0;
        private double dcOffsetY, prevDcY = 0;
        private double dcScalingFactor = 2.0;
        double thetaOff;
        private int[] ZX1, ZX2;
        private int time = 0;
        private Vector3 instrumentTip;
        private Vector3 prevInstrumentTip;
        private double vectorScale = 0;

        Matrix R = Matrix.Parse("0.9097    0.0311   -0.4142\r\n"+
                                "-0.0481    0.9984   -0.0305\r\n"+
                                "0.4126    0.0476    0.9097");

        Matrix T = Matrix.Parse("99.85624\r\n"+
                                "3.49518\r\n"+
                                "7.02600");

        Matrix A = Matrix.Parse("0.9731    0.0320   -0.3049\r\n"+
                                "-0.0647    1.0243   -0.0559\r\n"+
                                "0.3244    0.1055    0.9421");

        private Thread thr;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            /*double[] arr = new double[2000];
           var Watch = Stopwatch.StartNew();
           for (int j = 0; j < 1000000; j++ )
               for (int i = 0; i < 2000; i++)
               {
                   arr[i] = i + 5;

               }
           Watch.Stop();
           var elapsed = Watch.ElapsedMilliseconds;*/

            thr = new Thread(new ThreadStart(DaqInit));
            //timerPhase.Start();
            thr.Start();
            left = new Camera();
            right = new Camera();
            stageInitialized = false;
            isTracking = false;
            UpdateButtonStates();
            CameraForm cfrm = new CameraForm(left, right);
            cfrm.MdiParent = this;
            cfrm.Show();
            model = new CalibrationModel(17.5);
            calibForm = new CalibrationForm(left, right, model);
            calibForm.MdiParent = this;
            calibForm.Show();
            //file = new System.IO.StreamWriter(string.Format("data{0:yyyyMMddHHmm}.txt",DateTime.Now));
            origin = new Vector3(-66.5681286245652, 93.959147563999, 220.553692515875);
            trackForm = new TrackingForm();
            trackForm.MdiParent = this;

            instrumentTip = new Vector3();
            prevInstrumentTip = new Vector3();


        }

        private void DaqInit()
        {
            try
            {
                daqTask = new NationalInstruments.DAQmx.Task();
                daqTask2 = new NationalInstruments.DAQmx.Task();
                daqTask3 = new NationalInstruments.DAQmx.Task();
                daqTask.AIChannels.CreateVoltageChannel("Dev1/ai0", "", (AITerminalConfiguration)(-1), -10, 10, AIVoltageUnits.Volts);
                daqTask.AIChannels.CreateVoltageChannel("Dev1/ai1", "", (AITerminalConfiguration)(-1), -10, 10, AIVoltageUnits.Volts);
                daqTask2.AOChannels.CreateVoltageChannel("Dev1/ao0", "", -10, 10, AOVoltageUnits.Volts);
                daqTask2.AOChannels.CreateVoltageChannel("Dev1/ao1", "", -10, 10, AOVoltageUnits.Volts);
                daqTask.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, sampleRate);
                daqTask.AIChannels.All.DataTransferMechanism = AIDataTransferMechanism.Dma;

                daqTask2.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, sampleRate);
                daqTask2.AOChannels.All.DataTransferMechanism = AODataTransferMechanism.Dma;
                daqTask2.Stream.WriteRegenerationMode = WriteRegenerationMode.DoNotAllowRegeneration;
                reader = new AnalogMultiChannelReader(daqTask.Stream);
                int inBufSize1 = sampleRate, inBufSize2 = 2 * sampleRate;

                inDriveSignal = reader.ReadWaveform(inBufSize1);
                /*fftw_plan mplan;
                double[] din = new double[inBufSize1];
                double[] freq = new double[inBufSize1*2];
                GCHandle hdin = GCHandle.Alloc(din, GCHandleType.Pinned);
                GCHandle hdout = GCHandle.Alloc(freq, GCHandleType.Pinned);
                fftw_complexarray min = new fftw_complexarray(din);
                fftw_complexarray mout = new fftw_complexarray(freq);
                IntPtr fplan5 = fftw.dft_r2c_1d(sampleRate, hdin.AddrOfPinnedObject(), hdout.AddrOfPinnedObject(), fftw_flags.Estimate);
                Array.Copy(inDriveSignal[0].GetRawData(), din, inBufSize1);
                fftw.execute(fplan5);
                measuredFreq1 = (double)Util.AbsMaxIndex(freq) / 2;*/

                /*int rem = (int)((double)(measuredFreq1 - (int)measuredFreq1) * 1000);
                int mul;
                if (rem > 0)
                {
                    mul = 1000 / (int)Integers.GCD(rem, 1000);
                    measuredFreq1 *= mul;

                }*/

                //Array.Copy(inDriveSignal[1].GetRawData(), din, inBufSize1);
                //fftw.execute(fplan5);
                //measuredFreq2 = (double)Util.AbsMaxIndex(freq) / 2;
                ZX1 = Util.GetZeroCrossings(inDriveSignal[0].GetRawData());
                ZX2 = Util.GetZeroCrossings(inDriveSignal[1].GetRawData());
                /*rem = (int)((double)(measuredFreq2 - (int)measuredFreq2) * 1000);
                if (rem > 0)
                {
                    mul = 1000 / (int)Integers.GCD(rem, 1000);
                    measuredFreq2 *= mul;
                }*/
                //samplePerChannel = (int)Math.Max(ch1, ch2);
                //sampleRate = 500*(int)Integers.LCM((long)measuredFreq1, (long)measuredFreq2);
                samplePerChannel = sampleRate / 25;
                daqTask2.EveryNSamplesWrittenEventInterval = samplePerChannel;
                daqTask2.EveryNSamplesWritten += daqTask2_EveryNSamplesWritten;
                writer = new AnalogMultiChannelWriter(daqTask2.Stream);
                bufsize = sampleRate;

                waveX = new double[180][];
                waveY = new double[180][];
                outWave = new double[2, samplePerChannel];
                for (int i = 0; i < 180; i++)
                {
                    waveX[i] = new double[bufsize];
                    waveY[i] = new double[bufsize];
                }
                waveX[0] = inDriveSignal[0].GetRawData(0, bufsize);
                waveY[0] = inDriveSignal[1].GetRawData(0, bufsize);

                //Array.Copy(waveX[0], waveX[0].Length / 2, waveY[0], 0, waveY[0].Length / 2);
                //Array.Copy(waveX[0], 0, waveY[0], waveY[0].Length / 2, waveY[0].Length / 2);

                // initialize rotations
                for (int i = 1; i < 180; i++)
                {
                    for (int j = 0; j < bufsize; j++)
                    {
                        double theta = ((i) / 180.0) * Math.PI;
                        double x = waveX[0][j]; double y = waveY[0][j];
                        waveX[i][j] = x * Math.Cos(theta) - y * Math.Sin(theta);
                        waveY[i][j] = x * Math.Sin(theta) + y * Math.Cos(theta);
                    }
                }

                double[] X = new double[samplePerChannel];
                double[] Y = new double[samplePerChannel];
                
                Array.Copy(waveX[0], ZX1[1], X, 0, samplePerChannel);
                Array.Copy(waveY[0], ZX2[1], Y, 0, samplePerChannel);
                sampleIndex = samplePerChannel;
                sampleIndex2 = samplePerChannel;
                AnalogWaveform<double>[] waves = { AnalogWaveform<double>.FromArray1D(X), AnalogWaveform<double>.FromArray1D(Y) };
                writer.WriteWaveform<double>(false, waves);
                daqTask.Timing.ConfigureSampleClock("", sampleRate, SampleClockActiveEdge.Rising, SampleQuantityMode.HardwareTimedSinglePoint, 0);
                double[] sample;
                bool negative = false;

                daqTask2.Start();
                //daqTask2.AOChannels.All.DataTransferMechanism = AODataTransferMechanism.Dma;
                dcOffsetX = dcOffsetY = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void daqTask2_Done(object sender, TaskDoneEventArgs e)
        {
            daqTask2_EveryNSamplesWritten(sender, null);
        }
        private void queueMoreData(IAsyncResult ar)
        {
            sampleIndex += 10000;
            sampleIndex = sampleIndex % 200000;
            //double[] data = inDriveSignal[0].GetRawData(sampleIndex, 10000);
            //writer.BeginWriteWaveform<double>(true, AnalogWaveform<double>.FromArray1D(data), daqCallback, null);
        }

        void daqTask2_EveryNSamplesWritten(object sender, EveryNSamplesWrittenEventArgs e)
        {
            
            try
            {

                if (logTrackingData && file != null)
                {
                    file.Write( dcOffsetX + " " + dcOffsetY + " ");
                }

                int theta = (int)(360 - (phase-numericUpDown1.Value)) % 180;
                             

                if (logTrackingData && file != null)
                {
                    file.WriteLine(dcOffsetX + " " + dcOffsetY);
                }

                // dc offsets
                double filtdc = 0.6;
                thetaOff = ((double)numericUpDown1.Value * Math.PI / 180.0);
                double dx = dcOffsetX, dy = dcOffsetY;
                double threshold1 = 6.5, threshold2 = 1;
 
                double ct = Math.Cos(thetaOff);
                double st = Math.Sin(thetaOff);

                double tmpx = dx;
                dx = ct * dx + st * dy;
                dy = st * tmpx - ct * dy;

                dx = filtdc * (dx) + (1 - filtdc) * prevDcX;
                dy = filtdc * (dy) + (1 - filtdc) * prevDcY;

                if (dx < -1 * threshold1) dx = -1 * threshold1;
                else if (dx > threshold1) dx = threshold1;

                if (dy < -1 * threshold1) dy = -1 * threshold1;
                else if (dy > threshold1) dy = threshold1;

                dx = Math.Round(dx, 3);
                dy = Math.Round(dy, 3);
                prevDcX = dx;
                prevDcY = dy;

                int count = 0, i = sampleIndex, j = sampleIndex2;
                while (count <= samplePerChannel - 1)
                {
                    outWave[0, count] = waveX[theta][i] + dx;
                    outWave[1, count] = waveY[theta][i] + dy;
                    i++;
                    if (i >= ZX1[ZX1.Length - 3]) i = ZX1[1];
                    j++;
                    if (i >= ZX2[ZX2.Length - 3]) j = ZX2[1];
                    count++;
                }

                sampleIndex = i;
                sampleIndex2 = j;

                //AnalogWaveform<double>[] waves = { AnalogWaveform<double>.FromArray1D(X), AnalogWaveform<double>.FromArray1D(Y) };
                //writer.WriteWaveform<double>(true, waves);
                writer.WriteMultiSample(true, outWave);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void InitializeStage()
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
            Calibrate3XY();
        }

        private void Calibrate1()
        {
            int directionX = 1;
            int directionY = 1;

            double stepX = 0.1;
            double stepY = 0.1;
            double stepZ = 0.1;

            double maxX = 2.1;
            double maxY = 2.1;

            for (double z = 0; z < 0.6; z += stepZ)
            {
                for (double y = 0; y < maxY; y += stepY)
                {
                    for (double x = 0; x < maxX; x += stepX)
                    {
                        model.Move(directionX * stepX, 0, 0);
                        file.WriteLine(model.GetPoint(0).X + " " + model.GetPoint(0).Y + " " + model.GetPoint(0).Z + " " + left.points[0].X + " " + left.points[0].Y + " " + right.points[0].X + " " + right.points[0].Y);
                    }

                    file.Flush();

                    directionX *= -1;
                    maxX = (directionX < 0) ? 1.8 : 2.1;

                    model.Move(0, directionY * stepY, 0);
                }
                directionY *= -1;
                maxY = (directionX < 0) ? 1.8 : 2.1;

                model.Move(0, 0, stepZ);
            }

            file.Flush();
            file.Close();
        }

        private void Calibrate2()
        {
            Random rand = new Random();
            double resX = 100; // microns
            double resY = 100;
            double resZ = 500;
            double x, y, z;

            for (int i = 0; i < 8000; i++)
            {
                x = ConvertToPosition((double)rand.Next(25001), resX) / 1000;
                y = ConvertToPosition((double)rand.Next(25001), resY) / 1000;
                z = ConvertToPosition((double)rand.Next(21001), resZ) / 1000;

                model.MoveAbs(x, y, z);

                file.WriteLine(model.GetPoint(0).X + " " + model.GetPoint(0).Y + " " + model.GetPoint(0).Z + " " + left.points[0].X + " " + left.points[0].Y + " " + right.points[0].X + " " + right.points[0].Y);
                file.Flush();
            }
            file.Close();
        }

        private void Calibrate3XY()
        {
            MLApp.MLApp matlab;
            matlab = new MLApp.MLApp();
            matlab.Execute(@"cd G:\!Research\Tracking\instrument-tracking\TOOLBOX_calib");
            String l;
            String r;
            double x = 0, y = 0, z = 0;
            int pause = 0;
            for (z = 0; z <= 20; z += 4)
            {
                for (x = 0; x < 24; x += 0.2)
                {
                    model.MoveAbs(x, y, z);
                    Thread.Sleep(pause);
                    //l = "[" + left.points[0].X + ";" + left.points[0].Y + "]"; ;
                    // r = "[" + right.points[0].X + ";" + right.points[0].Y + "]";

                    //String ans = matlab.Execute("test_stereo_cs(" + l + "," + r + ")");
                    Vector3 XL = StereoTriangulation(left.points[0], right.points[0]);
                    //String[] res = ans.Split('\n');
                    file.WriteLine(model.GetPoint(0).X + " " + model.GetPoint(0).Y + " " + model.GetPoint(0).Z + " " + XL[0] + " " + XL[1] + " " + XL[2] + " " + left.points[0].X + " " + left.points[0].Y + " " + right.points[0].X + " " + right.points[0].Y);
                    file.Flush();
                }

                for (y = 0; y < 24; y += 0.2)
                {


                    model.MoveAbs(x, y, z);
                    Thread.Sleep(pause);
                    //l = "[" + left.points[0].X + ";" + left.points[0].Y + "]"; ;
                    //r = "[" + right.points[0].X + ";" + right.points[0].Y + "]";
                    //;
                    //String ans = matlab.Execute("test_stereo_cs(" + l + "," + r + ")");
                    Vector3 XL = StereoTriangulation(left.points[0], right.points[0]);
                    //String[] res = ans.Split('\n');
                    file.WriteLine(model.GetPoint(0).X + " " + model.GetPoint(0).Y + " " + model.GetPoint(0).Z + " " + XL[0] + " " + XL[1] + " " + XL[2] + " " + left.points[0].X + " " + left.points[0].Y + " " + right.points[0].X + " " + right.points[0].Y);
                    file.Flush();
                }

                //pause += 1000;
            }
            file.Close();
        }

        private void Calibrate3Z()
        {
            MLApp.MLApp matlab;
            matlab = new MLApp.MLApp();
            matlab.Execute(@"cd G:\!Research\Tracking\instrument-tracking\TOOLBOX_calib");
            String l;
            String r;
            double x = 0, y = 0, z = 0;

            for (z = 0; z <= 20; z += 0.5)
            {
                model.MoveAbs(0, 0, z);

                l = "[" + left.points[0].X + ";" + left.points[0].Y + "]"; ;
                r = "[" + right.points[0].X + ";" + right.points[0].Y + "]";

                String ans = matlab.Execute("test_stereo_cs(" + l + "," + r + ")");
                Vector3 XL = StereoTriangulation(left.points[0], right.points[0]);
                String[] res = ans.Split('\n');
                file.WriteLine(model.GetPoint(0).X + " " + model.GetPoint(0).Y + " " + model.GetPoint(0).Z + " " + res[3] + " " + res[4] + " " + res[5] + " " + left.points[0].X + " " + left.points[0].Y + " " + right.points[0].X + " " + right.points[0].Y);
                file.Flush();

            }
            file.Close();
        }

        private void CalibrateNN()
        {
            MLApp.MLApp matlab;
            matlab = new MLApp.MLApp();
            matlab.Execute(@"addpath('G:\!Research\Tracking\instrument-tracking\TOOLBOX_calib')");
            String l;
            String r;
            double x = 0, y = 0, z = 0;

            for (z = 0; z <= 20; z += 10)
            {
                for (y = 0; y < 24; y += 0.2)
                {


                    model.MoveAbs(x, y, z);

                    l = "[" + left.points[0].X + ";" + left.points[0].Y + "]"; ;
                    r = "[" + right.points[0].X + ";" + right.points[0].Y + "]";

                    String ans = matlab.Execute("test_stereo_net(" + l + "," + r + ")");
                    String[] res = ans.Split('\n');
                    file.WriteLine(model.GetPoint(0).X + " " + model.GetPoint(0).Y + " " + model.GetPoint(0).Z + " " + res[3] + " " + res[4] + " " + res[5] + " " + left.points[0].X + " " + left.points[0].Y + " " + right.points[0].X + " " + right.points[0].Y);
                    file.Flush();
                }

                for (x = 0; x < 24; x += 0.2)
                {
                    model.MoveAbs(x, y, z);

                    l = "[" + left.points[0].X + ";" + left.points[0].Y + "]"; ;
                    r = "[" + right.points[0].X + ";" + right.points[0].Y + "]";

                    String ans = matlab.Execute("test_stereo_net(" + l + "," + r + ")");
                    String[] res = ans.Split('\n');
                    file.WriteLine(model.GetPoint(0).X + " " + model.GetPoint(0).Y + " " + model.GetPoint(0).Z + " " + res[3] + " " + res[4] + " " + res[5] + " " + left.points[0].X + " " + left.points[0].Y + " " + right.points[0].X + " " + right.points[0].Y);
                    file.Flush();
                }


            }
            file.Close();
        }

        private Vector3 StereoTriangulation(Point2D left, Point2D right)
        {
            Vector3 res;

            Point2D fcl = new Point2D(1263.254, 1265.94);
            Point2D ccl = new Point2D(463.03, 289.98);
            double[] kcl = { 0.07175, 0.0825, -0.02280, -0.00874, 0 };
            Point2D fcr = new Point2D(1274.17, 1273.9);
            Point2D ccr = new Point2D(487.43, 296.302);
            double[] kcr = { 0.08770, -0.02617, -0.0203, -0.0077, 0 };

            Point2D _left = NormalizePixel(left, fcl, ccl, kcl);
            Point2D _right = NormalizePixel(right, fcr, ccr, kcr);

            Matrix xt = new Matrix(3, 1);
            xt[0, 0] = _left.X;
            xt[1, 0] = _left.Y;
            xt[2, 0] = 1;

            Matrix xtt = new Matrix(3, 1);
            xtt[0, 0] = _right.X;
            xtt[1, 0] = _right.Y;
            xtt[2, 0] = 1;

            Matrix u = R * xt;

            double n_xt2 = (xt[0, 0] * xt[0, 0] + xt[1, 0] * xt[1, 0] + xt[2, 0] * xt[2, 0]); // dot product;
            double n_xtt2 = (xtt[0, 0] * xtt[0, 0] + xtt[1, 0] * xtt[1, 0] + xtt[2, 0] * xtt[2, 0]); // dot product;
            double dot_xttu = (xtt[0, 0] * u[0, 0] + xtt[1, 0] * u[1, 0] + xtt[2, 0] * u[2, 0]);
            double dot_xttT = (xtt[0, 0] * T[0, 0] + xtt[1, 0] * T[1, 0] + xtt[2, 0] * T[2, 0]);
            double dot_uT = (u[0, 0] * T[0, 0] + u[1, 0] * T[1, 0] + u[2, 0] * T[2, 0]);

            double DD = n_xt2 * n_xtt2 - dot_xttu * dot_xttu;
            double NN1 = dot_xttu * dot_xttT - n_xtt2 * dot_uT;
            double NN2 = n_xt2 * dot_xttT - dot_uT * dot_xttu;

            double Zt = NN1 / DD;
            double Ztt = NN2 / DD;

            Matrix X1 = xt * Zt;
            Matrix tmp = (xtt * Ztt) - T;
            Matrix X2 = Matrix.Transpose(R) * ((xtt * Ztt) - T);

            Matrix XL = 0.5 * (X1 + X2);
            Matrix XR = R * XL + T;


            XL = A * XL;
            return new Vector3(XL[0, 0], XL[1, 0], XL[2, 0]);
        }

        private Point2D NormalizePixel(Point2D pixel, Point2D fc, Point2D cc, double[] kc)
        {
            Point2D xd = new Point2D();
            xd.X = (pixel.X - cc.X) / fc.X;
            xd.Y = (pixel.Y - cc.Y) / fc.Y;

            Point2D x = new Point2D(xd.X, xd.Y); // initial guess
            Point2D deltaX = new Point2D();

            for (int i = 0; i < 20; i++)
            {
                double r2 = x.X * x.X + x.Y * x.Y;
                double kRadial = 1 + kc[0] * r2 + kc[1] * Math.Pow(r2, 2) + kc[4] * Math.Pow(r2, 3);
                deltaX.X = 2 * kc[2] * x.X * x.Y + kc[3] * (r2 + 2 * x.X * x.X);
                deltaX.Y = 2 * kc[3] * x.X * x.Y + kc[2] * (r2 + 2 * x.Y * x.Y);

                x.X = (xd.X - deltaX.X) / kRadial;
                x.Y = (xd.Y - deltaX.Y) / kRadial;
            }

            return x;

        }
        /* Changes an arbitrary value 'val' so that val%res = 0 */
        private double ConvertToPosition(double val, double res)
        {
            return Math.Round(val / res) * res;
        }

        private void btnHomeStage_Click(object sender, EventArgs e)
        {
            motorX.MoveHome(false);
            motorY.MoveHome(false);
            motorZ.MoveHome(false);
        }
        private void btnTrack_Click(object sender, EventArgs e)
        {
            if (isTracking)
            {
                isTracking = false;
                dcOffsetX = dcOffsetY = 0;
                btnTrack.Text = "Start Tracking";

                if (logTrackingData)
                {
                    file.Flush();
                    file.Close();
                    file = null;
                }
                timerTracking.Stop();
            }
            else
            {
                isTracking = true;

                //set flag for computations, set to false upon first triangulation
                trackingStarted = 0;

                btnTrack.Text = "Stop Tracking";

                if (logTrackingData)
                {
                    file = new System.IO.StreamWriter(string.Format("data{0:yyyyMMddHHmm}.txt", DateTime.Now));
                }

                // Setup tracking form
                trackForm.Clear();
                trackForm.Show();

                timerTracking.Start();
            }
        }

        private void sortPoints(ref Point2D[] points)
        {
            Point2D tmp;
            var sorted = points.OrderBy(p => p.Y);
        }

        private void timerTracking_Tick(object sender, EventArgs e)
        {
            double filt = 0.5, filtdc = 0.5;
            Vector3 tip = CalculateInstrumentTip();
            if (tip.IsValid)
            {
                prevInstrumentTip = instrumentTip;
                instrumentTip = filt * (tip - origin) + (1 - filt) * prevInstrumentTip;
                //instrumentTip = tip - origin;
                int x = ((int)((instrumentTip[0])));
                int y = ((int)((instrumentTip[1])));
                dcOffsetX = instrumentTip[0] / dcScalingFactor;
                dcOffsetY = instrumentTip[1] / dcScalingFactor ;
                

                instrumentAngle = Math.Atan((tip[1] - top[1]) / (tip[0] - top[0])) * (180 / Math.PI);

                //if (instrumentAngle < 0) instrumentAngle += 180;
                phase = (int)Math.Round(instrumentAngle);
                //trackForm.DrawInstrument(top[0, 0], top[1, 0], bot[0, 0], bot[1, 0], tip[0, 0], tip[1, 0], instrumentAngle);
                trackForm.Draw(x,y, instrumentAngle);
                if (logTrackingData)
                {
                    file.WriteLine(instrumentTip[0] + " " + instrumentTip[1] + " " + dcOffsetX + " " +dcOffsetY + " " + phase + " " + thetaOff);
                }
            }
            else
            {
                //instrumentTip = prevInstrumentTip;
            }

            //Matrix lenM = tip - top;
            //int len = (int) Math.Sqrt(lenM[0,0]*lenM[0,0] + lenM[1,0]*lenM[1,0] + lenM[2,0]*lenM[2,0]);
            //trackForm.DrawLine(len);
        }

        private Vector3 CalculateInstrumentTip()
        {
            prevInstrumentTip = instrumentTip;
            sortPoints(ref left.points);
            sortPoints(ref right.points);
            bool invalidState = left.points[0].X == 1023 || left.points[1].X == 1023 || left.points[2].X == 1023 ||
                                right.points[0].X == 1023 || right.points[1].X == 1023 || right.points[2].X == 1023;
            if (invalidState) return new Vector3(0, 0, 0, false);

            top = StereoTriangulation(left.points[0], right.points[0]);
            mid = StereoTriangulation(left.points[1], right.points[1]);
            bot = StereoTriangulation(left.points[2], right.points[2]);

            Vector3 r = bot - mid;
            double rx = r[0], ry = r[1], rz = r[2];
            double px = top[0], py = top[1], pz = top[2];
            double r2 = rx * rx + ry * ry + rz * rz;
            Vector3 u = r * (1 / r2);
            double a = r2;
            double b = (px * rx + py * ry + pz * rz) * 2;
            double c = (px * px + py * py + pz * pz) - 17 * 17;
            if (trackingStarted <10)
            {
                double tmp = vectorScale;
                vectorScale = (rx * (bot[0] - top[0]) + ry * (bot[1] - top[1]) + rz * (bot[2] - top[2])) / r2;
                if ((tmp > 0))
                    vectorScale = 0.5 * (vectorScale + tmp);
                trackingStarted++;
            }
            double t1 = (-1 * b + Math.Sqrt(b * b - 4 * a * c)) / 2 * a;
            double t2 = (-1 * b - Math.Sqrt(b * b - 4 * a * c)) / 2 * a;
            t2 = Math.Sqrt(130 * 130);

            //Matrix tip = bot + (6.4/6.3)*((bot - top));
            //Matrix tip = bot + (4.8 / 7) * ((bot - top));
            Vector3 tip = (top + r*((122+38.9)/122)*vectorScale); // pencil
            //Vector3 tip = (top + r * (l1/l2) * vectorScale);
            //Vector3 tip = (top + r * (119.9 / 53.4) * vectorScale); // soft tip and straight canula
             //Vector3 tip = (top + r * (114/(53.5)) * vectorScale); // curved canula with adaptor
           //Vector3 tip = (top + r * (126 / 53.5) * vectorScale); // needle tip
            //Matrix tip = (top + u * Math.Sqrt(140 * 140));
            return tip;

        }

        private Vector3 CalculateInstrumentTipCalib()
        {
            prevInstrumentTip = instrumentTip;

            bool invalidState = left.points[0].X == 1023 || left.points[1].X == 1023 || left.points[2].X == 1023 ||
                                right.points[0].X == 1023 || right.points[1].X == 1023 || right.points[2].X == 1023;
            if (invalidState) return new Vector3(0, 0, 0, false); ;

            double plp = 0, pln = 0, pq = 0, pl1 = 0, pl2 = 0;

            for (int i = 0; i < 100000; i++)
            {
                top = StereoTriangulation(left.points[0], right.points[0]);
                mid = StereoTriangulation(left.points[1], right.points[1]);
                bot = StereoTriangulation(left.points[2], right.points[2]);
                instrumentTip = StereoTriangulation(left.points[3], right.points[3]);

                Vector3 ncap = Vector3.CrossProduct(bot - top, mid - top);
                ncap.Normalize();
                Vector3 tT = instrumentTip - top;
                ln = Vector3.DotProduct(tT, ncap); // normal component

                Vector3 tTp = tT - ln * ncap;
                lp = tTp.Magnitude; // plane component

                Vector3 tTpcap = tTp / tTp.Magnitude;
                Vector3 bmcap = (mid - bot) / (mid - bot).Magnitude;
                Vector3 tmp = Vector3.CrossProduct(top - bot, tTpcap) / Vector3.CrossProduct(bmcap, tTpcap);
                q = tmp[0];
                tT.Normalize();
                l2 = Vector3.DotProduct((bot - top), tT);
                l1 = (instrumentTip - top).Magnitude;
                if (i > 0)
                {
                    q = (q + pq) / 2;
                    ln = (ln + pln) / 2;
                    lp = (lp + plp) / 2;
                    l1 = (l1 + pl1) / 2;
                    l2 = (l2 + pl2) / 2;
                }

                plp = lp; pln = ln; pq = q; pl1 = l1; pl2 = l2;

            }

            return new Vector3();

        }

        private Vector3 CalculateInstrumentTip2()
        {
            prevInstrumentTip = instrumentTip;

            bool invalidState = left.points[0].X == 1023 || left.points[1].X == 1023 || left.points[2].X == 1023 ||
                                right.points[0].X == 1023 || right.points[1].X == 1023 || right.points[2].X == 1023;
            if (invalidState) return new Vector3(0, 0, 0, false); ;

            top = StereoTriangulation(left.points[0], right.points[0]);
            mid = StereoTriangulation(left.points[1], right.points[1]);
            bot = StereoTriangulation(left.points[2], right.points[2]);

            Vector3 n = Vector3.CrossProduct(bot - top, mid - top);
            Vector3 ncap = n / n.Magnitude;
            Vector3 bmcap = (mid - bot) / (mid - bot).Magnitude;
            Vector3 tT = bot + q * bmcap - top;
            tT = -1 * (tT / tT.Magnitude) * lp;

            return (top + (ln * ncap + tT));

        }

        private void timerPhase_Tick(object sender, EventArgs e)
        {
            if (waveX == null) return;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //phase = (int)numericUpDown1.Value;
        }

        private void btnSetOrigin_Click(object sender, EventArgs e)
        {
            trackingStarted = 0;
            origin = CalculateInstrumentTip();

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            dcScalingFactor = (double)numericUpDown2.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            logTrackingData = checkBox1.Checked;
        }

        private void btnCalibInstrument_Click(object sender, EventArgs e)
        {
            CalculateInstrumentTipCalib();
            AutoClosingMessageBox.Show("Calibration Done", "Notification", 1000);
        }

    }
}
