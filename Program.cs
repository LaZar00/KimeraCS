using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimeraCS
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.AddMessageFilter(new MenuFilter()); // Add a message filter for avoid Menu key.
            Application.Run(new FrmSkeletonEditor());
        }
    }

    //public class MenuFilter : IMessageFilter
    //{
    //    public bool PreFilterMessage(ref Message m)
    //    {
    //        const int WM_SYSKEYDOWN = 0x0104;
    //        if (m.Msg == WM_SYSKEYDOWN)
    //        {
    //            bool alt = ((int)m.LParam & 0x20000000) != 0;
    //            if (alt && (m.WParam == new IntPtr((int)Keys.Menu)))
    //                return true; // eat it!                
    //        }
    //        return false;
    //    }
    //}
}

