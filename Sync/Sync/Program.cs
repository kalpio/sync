using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sync
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{917628D7-2BF2-4988-86F4-1F7918AB8771}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            else
            {
                MessageBox.Show("Program jest już uruchomiony.");
            }
        }
    }
}
