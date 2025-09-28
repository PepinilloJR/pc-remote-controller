using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controller;
namespace Controllers
{
    public class WindowsController : Controller.Controller
    {
        // importacion de metodos del windows API

        public struct POINT
        {
            public int x;
            public int y;
        }

        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32")]
        public static extern bool GetCursorPos(out POINT posMouse);

        [DllImport("user32")]
        public static extern int GetSystemMetrics(int nIndex);

        //79 virtual height
        //78 virual width

        [DllImport("user32")]
        public static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        public string getType()
        {
            return "WindowsController";
        }

        public static void WriteText()
        {

        }

        public static void WriteText(string input)
        {

        }

        public static void MoveMouse(string direction)
        {
            GetCursorPos(out POINT pt);
            IntPtr monitorHandle = MonitorFromPoint(pt, 2);
            Console.WriteLine(monitorHandle);

            MONITORINFO mi = new MONITORINFO();
            mi.cbSize = Marshal.SizeOf(mi);

            GetMonitorInfo(monitorHandle, ref mi);

            Console.WriteLine(mi.rcMonitor.Top);
        }
    }


}
