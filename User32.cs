using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace KimeraCS
{
    class User32
    {
        internal const string DLLName = "USER32.DLL";

        [DllImport(DLLName, EntryPoint = "GetDC", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport(DLLName, EntryPoint = "ReleaseDC")]
        public static extern Int32 ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DLLName, EntryPoint = "BlockInput", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void BlockInput([In, MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        // FolderBrowserDialogEx
        [DllImport(DLLName, EntryPoint = "SendMessageW")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, uint msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        [DllImport(DLLName, EntryPoint = "FindWindowExW")]
        public static extern IntPtr FindWindowExW(IntPtr hWndParent, IntPtr hWndChildAfter, [MarshalAs(UnmanagedType.LPWStr)] string lpszClass, [MarshalAs(UnmanagedType.LPWStr)] string lpszWindow);
        [DllImport(DLLName, EntryPoint = "GetActiveWindow")]
        public static extern IntPtr GetActiveWindow();
        [DllImport(DLLName, EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
    }
}
