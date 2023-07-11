using System;
using System.Windows.Forms;

namespace ZKTecoFingerPrintScanner_Implementation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new LoadingForm());
            Application.Run(new ScreenHome());

        }
    }
}
