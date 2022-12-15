using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace FEFTHost
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{6984d88a-60ab-4ea5-93cc-895e482f4a21}");
        [STAThread]
        static void Main()
        {
            //DateTime dt;
            //dt = new DateTime(2015, 4, 15);
            //if (DateTime.Now.Date <= dt)
            //{

                if (mutex.WaitOne(TimeSpan.Zero, true))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmFEFTHost());
                    mutex.ReleaseMutex();
                }
                else
                {
                    MessageBox.Show("Another instance is of already Running. Only one instance is allowed at a time", "FEFT Webservice");
                }
            //}
            //else
            //{
            //    MessageBox.Show("The webservice has expired, please contact the vendor:danoyuga@gmail.com", "FEFT Webservice");
            //}
        }
    }
}
