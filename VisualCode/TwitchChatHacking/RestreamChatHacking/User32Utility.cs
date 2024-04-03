using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RestreamChatHacking
{
    class User32Utility
    {
        public static void HideTheInterfaceWithUser32DLL()
        {
            var openWindows = Process.GetProcesses().Where(process => String.IsNullOrEmpty(process.MainWindowTitle) == false);
            SetWindowPos(Process.GetCurrentProcess().MainWindowHandle, IntPtr.Zero, 0, -2000, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            ShowWindowAsync(Process.GetCurrentProcess().MainWindowHandle, SW_SHOWMINIMIZED);
        }


        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


    }
}
