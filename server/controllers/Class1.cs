using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public enum VirtualKeys
    {
        // letters
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        // Numbers
        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,

        // control
        Enter = 0x0D,
        Esc = 0x1B,
        Space = 0x20,
        Tab = 0x09,
        Backspace = 0x08,
        Shift = 0x10,
        Ctrl = 0x11,
        Alt = 0x12,
        CapsLock = 0x14,

        // Nav
        Left = 0x25,
        Up = 0x26,
        Right = 0x27,
        Down = 0x28,
        Home = 0x24,
        End = 0x23,
        PageUp = 0x21,
        PageDown = 0x22,
        Insert = 0x2D,
        Delete = 0x2E,

        // Function
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,

        // Numpad
        Num0 = 0x60,
        Num1 = 0x61,
        Num2 = 0x62,
        Num3 = 0x63,
        Num4 = 0x64,
        Num5 = 0x65,
        Num6 = 0x66,
        Num7 = 0x67,
        Num8 = 0x68,
        Num9 = 0x69,
        NumMultiply = 0x6A,
        NumAdd = 0x6B,
        NumSubtract = 0x6D,
        NumDecimal = 0x6E,
        NumDivide = 0x6F,

        // Common
        PrintScreen = 0x2C,
        ScrollLock = 0x91,
        Pause = 0x13,
        NumLock = 0x90,
        WinLeft = 0x5B,
        WinRight = 0x5C,
        Menu = 0x5D,

        vol_down = 0xAE,
        vol_up = 0xAF
}
}
