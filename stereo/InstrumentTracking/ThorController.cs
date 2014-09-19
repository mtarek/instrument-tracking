using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FTD2XX_NET;
using System.Runtime.InteropServices;

namespace InstrumentTracking
{

    public enum ThorStageModel
    {
        MTS25_Z8,
        TOTAL_AVAILABLE,
    }

    class ThorStageFactors
    {
        private double encCnt;
        private double position;
        private double velocity;
        private double acceleration;

        public ThorStageFactors(double enc, double pos, double vel, double acc)
        {
            encCnt = enc;
            position = pos;
            velocity = vel;
            acceleration = acc;
        }

        public double EncCnt
        {
            get { return encCnt; }
        }

        public double Position
        {
            get { return position; }
        }

        public double Velocity
        {
            get { return velocity; }
        }

        public double Acceleration
        {
            get { return acceleration; }
        }

    }

    class ThorStage
    {
        private ThorStageModel stageModel;
        private int modelIndex;

        private ThorStageFactors[] factorsTable;

        public ThorStageFactors ScaleFactors;

        public ThorStage(ThorStageModel model)
        {
            stageModel = model;

            factorsTable = new ThorStageFactors[(int)ThorStageModel.TOTAL_AVAILABLE];
            
            modelIndex = (int) model;

            /* Setup model factors here */
            factorsTable[(int)ThorStageModel.MTS25_Z8] = new ThorStageFactors(34304, 34304, 767367.49, 261.93);
            /* End model factors */

            ScaleFactors = factorsTable[modelIndex];
        }

    }


    class ThorController
    {
        string serialNumber;
        FTDI deviceHandle;
        FTDI.FT_STATUS ftStatus;
        int statusBits;
        ThorStage stage;

        ThorStageFactors[] factors;

        public ThorController(ThorStageModel stageModel, string serialNumber)
        {
            stage = new ThorStage(stageModel);

            UInt32 deviceCount = 0;
            uint numBytesWritten = 0;

            this.serialNumber = serialNumber;
            deviceHandle = new FTDI();

            // Determine the number of FTDI devices connected to the machine
            ftStatus = deviceHandle.GetNumberOfDevices(ref deviceCount);

            // Populate device list
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[deviceCount];
            ftStatus = deviceHandle.GetDeviceList(ftdiDeviceList);

            ftStatus = deviceHandle.OpenBySerialNumber(serialNumber);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                throw new Exception("Failed to open FTDI device " + ftStatus.ToString());
            }


            // Set baud rate to 115200. 
            ftStatus = deviceHandle.SetBaudRate(115200);

            // 8 data bits, 1 stop bit, no parity 
            ftStatus = deviceHandle.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);

            // Pre purge dwell 50ms. 
            Thread.Sleep(50);

            // Purge the device. 
            ftStatus = deviceHandle.Purge(FTDI.FT_PURGE.FT_PURGE_RX | FTDI.FT_PURGE.FT_PURGE_TX);

            // Post purge dwell 50ms. 
            Thread.Sleep(50);

            // Reset device. 
            ftStatus = deviceHandle.ResetDevice();

            // Set flow control to RTS/CTS. 
            ftStatus = deviceHandle.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0, 0);

            // Set RTS. 
            ftStatus = deviceHandle.SetRTS(true);

            // Disable then enable channel
            byte[] change_enstate = { 0x10, 0x02, 0x01, 0x02, 0x50, 0x1 };
            ftStatus = deviceHandle.Write(change_enstate, 6, ref numBytesWritten);
            change_enstate[3] = 0x01;
            ftStatus = deviceHandle.Write(change_enstate, 6, ref numBytesWritten);

            // Move home
            byte[] movehome = { 0x43, 0x04, 0x01, 0x00, 0x50, 0x01 };
            /*ftStatus = deviceHandle.Write(movehome, 6, ref numBytesWritten);
            Thread.Sleep(3000);
            do
            {
                statusBits = GetStatus();
                //Console.WriteLine("{0:X}", statusbits);
            } while ((statusBits & 0x400) != 0x400 || statusBits == 0);*/

        }

        [StructLayout(LayoutKind.Sequential)]
        struct Message
        {
            public UInt16 messageID;
            public Byte param1;
            public Byte param2;
            public Byte dest;
            public Byte source;
        }

        private static Byte[] SerializeMessage<T>(T msg) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            Byte[] ret = new Byte[objsize];
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.StructureToPtr(msg, buff, true);
            Marshal.Copy(buff, ret, 0, objsize);
            Marshal.FreeHGlobal(buff);
            return ret;
        }
        

        int GetStatus()
        {
            uint numWritten = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            byte[] msg = { 0x90, 0x4, 0, 0, 0x50, 0x1 };
            byte[] header = new byte[6];
            byte[] response = new byte[20];
            for (int i = 0; i < 10; i++)
            {
                ftStatus = deviceHandle.Write(msg, 6, ref numWritten);
                numWritten = 0;
                ftStatus = deviceHandle.Read(header, 6, ref numWritten);
                Console.WriteLine("msg: 0x{0:X}", (header[1] << 8 | header[0]));
                byte dest = header[4];
                int len = header[3] << 8 | header[2];
                if (dest > 50)
                {
                    ftStatus = deviceHandle.Read(response, (uint)len, ref numWritten);

                    //ACK
                    msg[0] = 0x92;
                    ftStatus = deviceHandle.Write(msg, 6, ref numWritten);
                    return (response[13] << 24 | response[12] << 16 | response[11] << 8 | response[10]);
                }
                Thread.Sleep(1000);
            }

            return 0;
        }

        byte[] GetDCStatus()
        {
            byte[] response = new byte[20];
            uint numBytesWritten = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            Message msg = new Message();

            msg.messageID = 0x490;
            msg.param1 = 0; msg.param2 = 0;
            msg.source = 0x01; msg.dest = 0x50;

            ftStatus = deviceHandle.Write(SerializeMessage<Message>(msg), 6, ref numBytesWritten);

            ftStatus = deviceHandle.Read(response, 20, ref numBytesWritten);

            //ACK
            msg.messageID = 0x492;
            ftStatus = deviceHandle.Write(SerializeMessage<Message>(msg), 6, ref numBytesWritten);

            return response;
        }

        int GetVelocity(byte[] response)
        {
            //TODO FIX (PROBABLY TOTALLY WRONG)
            return (response[13] << 8 | response[12]) * 100 / 20;

        }

        double GetPosition(byte[] response)
        {

            int pos = response[11] << 24 |
                        response[10] << 16 |
                        response[9] << 8 |
                        response[8];
            return ((double)pos / stage.ScaleFactors.Position);
        }

        public void Move(double mmdistance)
        {
            uint numBytesWritten = 0;
            // Set Move Rel params to 100 mm distance
            // pos = posScaleFactor  * 100
            byte[] pos = BitConverter.GetBytes(stage.ScaleFactors.Position * mmdistance);
            byte[] moverelparams = { 0x45, 0x04, 0x06, 0x00, 0xA2, 0x1, 0x1, 0, pos[4], pos[5], pos[6], pos[7] };
            ftStatus = deviceHandle.Write(moverelparams, 12, ref numBytesWritten);

            // Move rel
            byte[] moverel = { 0x48, 0x04, 0x01, 0, 0x50, 0x1 };
            int k = 0;
            ftStatus = deviceHandle.Write(moverel, 6, ref numBytesWritten);
            while ((statusBits & 0x10) == 0)
            {
                statusBits = GetStatus();
            }
            do
            {
                statusBits = GetStatus();
                byte[] res = GetDCStatus();
                //Console.WriteLine("{0}: {1}", GetPosition(res), GetVelocity(res));
            } while ((statusBits & 0x10) != 0);

        }

        ~ThorController()
        {
            deviceHandle.Close();
        }
    }
}
