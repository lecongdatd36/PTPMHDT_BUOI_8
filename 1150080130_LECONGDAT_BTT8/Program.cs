using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySach;  // ✅ thêm dòng này

namespace _1150080130_LECONGDAT_BTT8
{
    internal static class Program
    {
      
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

        
            Application.Run(new Form4());
        }
    }
}
