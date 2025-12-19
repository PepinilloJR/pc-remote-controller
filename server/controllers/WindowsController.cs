using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
        static string lastInputClick = "";
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
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint wData, UIntPtr dwExtraInfo);
        // events 
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        [StructLayout(LayoutKind.Sequential)] // atributes of the struct in memory will be in order of definition 
        struct INPUT
        {
            public uint Type; // 2 is keyboard input
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)] // allows to control the position in memory of the fields
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            // field offset indicates the position in memory inside the struct of the field, in a Explicit layout
           
            [FieldOffset(0)]  public HARDWAREINPUT Hardware;

            [FieldOffset(0)] public KEYBDINPUT Keyboard; // i only use this

            [FieldOffset(0)] public MOUSEINPUT Mouse;

            // all this simulates an union, the API will use only one of the values depending in the Type flag on INPUT
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            // importante note: ushort for WORD and uint for DWORD. -> documentation equivalent on C++

            public ushort Vk; 
            public ushort Scan;
            public uint Flags; // putting this 0x0004 will allow to send unicode chars directly into scan, instead of using Virtual-Key Codes
            public uint Time;
            public IntPtr ExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);



        public string getType()
        {
            return "WindowsController";
        }

        public static void WriteText(char input)
        {
            INPUT ex = new INPUT();
            INPUT[] exampleInput = { ex };
            exampleInput[0].Type = 1;
            exampleInput[0].Data.Keyboard.Vk = 0;
            exampleInput[0].Data.Keyboard.Flags = 0x0004;
            exampleInput[0].Data.Keyboard.Scan = input; // char can directly be converted to unicode 
            exampleInput[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            exampleInput[0].Data.Keyboard.Time = 0;
            Console.WriteLine(input);
            SendInput(1, exampleInput, Marshal.SizeOf(typeof (INPUT)));

        }

        public static void WriteTextSpecial(string input)
        {
            INPUT ex = new INPUT();
            INPUT[] exampleInput = { ex };
            exampleInput[0].Type = 1;

            Enum.TryParse(input, out VirtualKeys vk);
            exampleInput[0].Data.Keyboard.Vk = ((char)vk);
            exampleInput[0].Data.Keyboard.Flags = 0;
            exampleInput[0].Data.Keyboard.Scan = 0; // char can directly be converted to unicode 
            exampleInput[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            exampleInput[0].Data.Keyboard.Time = 0;
            Console.WriteLine(input);
            SendInput(1, exampleInput, Marshal.SizeOf(typeof(INPUT)));

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

            Console.WriteLine(direction);
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

            } else if (button.Equals("right_click"))
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)pt.x, (uint)pt.y, 0, 0);

            } else if (button.Equals("right_click_up") && !lastInputClick.Equals("right_click_up"))
            {
                mouse_event(MOUSEEVENTF_RIGHTUP, (uint)pt.x, (uint)pt.y, 0, 0);

            }
            else if (button.Equals("left_click_up") && !lastInputClick.Equals("left_click_up"))
            {
                mouse_event(MOUSEEVENTF_LEFTUP, (uint)pt.x, (uint)pt.y, 0, 0);

            } else if (button.Contains("wheel_up"))
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 50, 0);
            }
            else if (button.Contains("wheel_down"))
            {
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (unchecked((uint)(-120))), 0);
            }

            lastInputClick = button;
        }

        public static void VolumenChange (string vol_dir)
        {
            // this bifurcation is redundant, but later on will make sense when direct volume control being implemented so idk :b
            if (vol_dir == "vol_up")
            {
                WriteTextSpecial(vol_dir);

            } else if (vol_dir == "vol_down")
            {
                WriteTextSpecial(vol_dir);
            }
        }
    }


}
