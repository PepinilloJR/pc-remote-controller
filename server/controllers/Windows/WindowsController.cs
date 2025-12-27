using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Controller;
namespace Controller
{
    public class WindowsController : IControllerBase
    {
        // windows API imports

        int accelerationCounter;
        int aceleration;
        string lastInput;
        string lastInputClick;


        public WindowsController()
        {
            accelerationCounter = 0;
            aceleration = 5;
            lastInput = "";
            lastInputClick = "";
        }


        public string getType()
        {
            return "WindowsController";
        }

        public void WriteText(string input)
        {
            Sys32.INPUT ex = new Sys32.INPUT();
            Sys32.INPUT[] exampleInput = { ex };
            exampleInput[0].Type = 1;
            exampleInput[0].Data.Keyboard.Vk = 0;
            exampleInput[0].Data.Keyboard.Flags = 0x0004;
            exampleInput[0].Data.Keyboard.Scan = input.ToCharArray()[0]; // char can directly be converted to unicode 
            exampleInput[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            exampleInput[0].Data.Keyboard.Time = 0;
            Console.WriteLine(input);
            Sys32.SendInput(1, exampleInput, Marshal.SizeOf(typeof(Sys32.INPUT)));

        }

        public void WriteTextSpecial(string input)
        {
            Sys32.INPUT ex = new Sys32.INPUT();
            Sys32.INPUT[] exampleInput = { ex };
            exampleInput[0].Type = 1;

            Enum.TryParse(input, true, out VirtualKeys vk);
            exampleInput[0].Data.Keyboard.Vk = ((char)vk);
            exampleInput[0].Data.Keyboard.Flags = 0;
            exampleInput[0].Data.Keyboard.Scan = 0; // char can directly be converted to unicode 
            exampleInput[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            exampleInput[0].Data.Keyboard.Time = 0;
            Console.WriteLine(input);
            Sys32.SendInput(1, exampleInput, Marshal.SizeOf(typeof(Sys32.INPUT)));

        }

        public void MoveMouse(string direction)
        {
            Sys32.GetCursorPos(out Sys32.POINT pt);

            Console.WriteLine(direction);
            // probably better to do this from the client
            if (lastInput.Equals(direction))
            {
                accelerationCounter += 1;
            }
            else
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
                Sys32.SetCursorPos(pt.x - aceleration, pt.y);
            }
            else if (direction.Equals("right"))
            {
                Sys32.SetCursorPos(pt.x + aceleration, pt.y);
            }
            else if (direction.Equals("down"))
            {
                Sys32.SetCursorPos(pt.x, pt.y + aceleration);
            }
            else if (direction.Equals("up"))
            {
                Sys32.SetCursorPos(pt.x, pt.y - aceleration);

            }
        }

        public void ClickMouse(string button)
        {
            Sys32.GetCursorPos(out Sys32.POINT pt);


            if (button.Equals("left_click"))
            {
                Sys32.mouse_event(Sys32.MOUSEEVENTF_LEFTDOWN, (uint)pt.x, (uint)pt.y, 0, 0);

            }
            else if (button.Equals("right_click"))
            {
                Sys32.mouse_event(Sys32.MOUSEEVENTF_RIGHTDOWN, (uint)pt.x, (uint)pt.y, 0, 0);

            }
            else if (button.Equals("right_click_up") && !lastInputClick.Equals("right_click_up"))
            {
                Sys32.mouse_event(Sys32.MOUSEEVENTF_RIGHTUP, (uint)pt.x, (uint)pt.y, 0, 0);

            }
            else if (button.Equals("left_click_up") && !lastInputClick.Equals("left_click_up"))
            {
                Sys32.mouse_event(Sys32.MOUSEEVENTF_LEFTUP, (uint)pt.x, (uint)pt.y, 0, 0);

            }
            else if (button.Contains("wheel_up"))
            {
                Sys32.mouse_event(Sys32.MOUSEEVENTF_WHEEL, 0, 0, 50, 0);
            }
            else if (button.Contains("wheel_down"))
            {
                Sys32.mouse_event(Sys32.MOUSEEVENTF_WHEEL, 0, 0, (unchecked((uint)(-120))), 0);
            }

            lastInputClick = button;
        }

        public void VolumenChange(string vol_dir)
        {
            // this bifurcation is redundant, but later on will make sense when direct volume control being implemented so idk :b
            if (vol_dir == "vol_up")
            {
                WriteTextSpecial(vol_dir);

            }
            else if (vol_dir == "vol_down")
            {
                WriteTextSpecial(vol_dir);
            }
        }
    }


}
