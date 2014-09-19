using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FTD2XX_NET;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace InstrumentTracking
{
    
    class Program
    {
        

        [STAThread]
        static void Main(string[] args)
        {

            MainForm form = new MainForm();
            Application.Run(form);


        }   

        
    }


}
