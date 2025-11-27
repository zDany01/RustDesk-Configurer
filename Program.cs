using System;
using System.Windows.Forms;

namespace RustDesk_Configurer
{
    internal static class Program
    {
        public const string APP_NAME = "RustDesk Configurer";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI());
        }
    }
}
