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
        // windows API imports

        static int accelerationCounter = 0;
        static int aceleration = 5;
        static string lastInput = "";

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

        // for mouse clicking
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
        // events 
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;


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
            
            // not needed
            /*
            IntPtr monitorHandle = MonitorFromPoint(pt, 2);
            //Console.WriteLine(monitorHandle);

            MONITORINFO mi = new MONITORINFO();
            mi.cbSize = Marshal.SizeOf(mi);

            GetMonitorInfo(monitorHandle, ref mi);
            int resolucionVirtual = GetSystemMetrics(78);
            Console.WriteLine(direction);
            */

            // probably better to do this from the client
            if (lastInput.Equals(direction))
            {
                accelerationCounter += 1;
            } else
            {
                accelerationCounter = 0;
                aceleration = 5;
            }
            if (accelerationCounter > 5)
            {
                aceleration = 10;
            }
            if (accelerationCounter > 10)
            {
                aceleration = 15;
            }
            if (accelerationCounter > 15)
            {
                aceleration = 25;
            }
            lastInput = direction;


            if (direction.Equals("left"))
            {



                SetCursorPos(pt.x - aceleration, pt.y);
            } else if (direction.Equals("right"))
            {
                SetCursorPos(pt.x + aceleration, pt.y);
            } else if (direction.Equals("down")) {
                SetCursorPos(pt.x, pt.y + aceleration);
            } else if (direction.Equals("up"))
            {
                SetCursorPos(pt.x, pt.y - aceleration);

            }
        }

        public static void ClickMouse (string button)
        {
            GetCursorPos(out POINT pt);


            if (button.Equals("left_click"))
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)pt.x, (uint)pt.y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, (uint)pt.x, (uint)pt.y, 0, 0);

            } else if (button.Equals("right_click"))
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)pt.x, (uint)pt.y, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, (uint)pt.x, (uint)pt.y, 0, 0);

            }
        }
    }


}
