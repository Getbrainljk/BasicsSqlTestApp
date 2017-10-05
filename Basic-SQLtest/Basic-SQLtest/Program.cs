using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 
    Basic_SQLtest
{
    static class 
        Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void 
            Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            App f1 = new App();
            f1.Text = "Sql-server test - arbia nadir";
            Application.Run(f1);
            return;
        }
    }
}
