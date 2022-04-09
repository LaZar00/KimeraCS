// Set a folder view at any subdirectory eg e:\Windows
//
// Create a new form in your project - call it FolderViewRoot
// edit the FolderViewRoot.cs and paste the contents of this file in.
//
// to use on your program use the following code
//
// - FolderViewRoot secondForm = new FolderViewRoot();
// - secondForm.FVrootdirectory = "c:\ATI";
// - secondForm.FVrootdirectorylable = "Select Media Directory";
// - secondForm.Show();
// -
// - // returns string in FVrootdirectoryfull
// - MessageBox.Show(secondForm.FVrootdirectoryfull);
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace KimeraCS
{

    using static User32;

    public class FolderBrowserDialogEX
    {

        // -----------------------------------------------------------------------------
        //  Helper for FolderBrowserDialog object.
        // -----------------------------------------------------------------------------
        public FolderBrowserDialog folderBrowser;
        public bool Disposed { get; private set; }

        public FolderBrowserDialogEX()
        {
            Tmr = new Timer() { Interval = 200 };
            folderBrowser = new FolderBrowserDialog();
        }

        public void Dispose()
        {
            if (Disposed) return;

            Disposed = true;

            Tmr.Dispose();
            folderBrowser.Dispose();
        }

        private Timer _Tmr;

        public Timer Tmr
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Tmr;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Tmr != null)
                {
                    _Tmr.Tick -= Tmr_Tick;
                }

                _Tmr = value;
                if (_Tmr != null)
                {
                    _Tmr.Tick += Tmr_Tick;
                }
            }
        }

        private const int WM_USER = 1024;
        private const int BFFM_SETEXPANDED = (WM_USER + 106);
        private const int WM_SETFOCUS = 7;
        private const int WM_SETREDRAW = 11;

        public void Tmr_Tick(object sender, System.EventArgs e)
        {
            // Dim hFb As IntPtr = FindWindowExW(IntPtr.Zero, IntPtr.Zero, "#32770", Nothing)
            IntPtr hFb = GetActiveWindow();

            SendMessage(hFb, WM_SETREDRAW, false, 0);
            if ((hFb != IntPtr.Zero))
            {
                IntPtr hChild = FindWindowExW(hFb, IntPtr.Zero, null, null);
                IntPtr hTreeView = FindWindowExW(hChild, IntPtr.Zero, "SysTreeView32", null);
                while ((hTreeView == IntPtr.Zero))
                {
                    hChild = FindWindowExW(hFb, hChild, null, null);
                    hTreeView = FindWindowExW(hChild, IntPtr.Zero, "SysTreeView32", null);
                }

                if (SendMessageW(hFb, BFFM_SETEXPANDED, 1, folderBrowser.SelectedPath) == IntPtr.Zero)
                {
                    Tmr.Stop();
                    SendMessageW(hTreeView, WM_SETFOCUS, 0, null);
                }

            }

            SendMessage(hFb, WM_SETREDRAW, true, 0);
        }
    }
}


