using System.Runtime.InteropServices;

namespace Controller
{
    public class Sys32
    {
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
        public const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        public const uint MOUSEEVENTF_LEFTUP = 0x04;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const uint MOUSEEVENTF_RIGHTUP = 0x10;
        public const uint MOUSEEVENTF_WHEEL = 0x0800;

        [StructLayout(LayoutKind.Sequential)] // atributes of the struct in memory will be in order of definition 
        public struct INPUT
        {
            public uint Type; // 2 is keyboard input
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)] // allows to control the position in memory of the fields
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            // field offset indicates the position in memory inside the struct of the field, in a Explicit layout
           
            [FieldOffset(0)]  public HARDWAREINPUT Hardware;

            [FieldOffset(0)] public KEYBDINPUT Keyboard; // i only use this

            [FieldOffset(0)] public MOUSEINPUT Mouse;

            // all this simulates an union, the API will use only one of the values depending in the Type flag on INPUT
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            // importante note: ushort for WORD and uint for DWORD. -> documentation equivalent on C++

            public ushort Vk; 
            public ushort Scan;
            public uint Flags; // putting this 0x0004 will allow to send unicode chars directly into scan, instead of using Virtual-Key Codes
            public uint Time;
            public IntPtr ExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }


        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

    }

}