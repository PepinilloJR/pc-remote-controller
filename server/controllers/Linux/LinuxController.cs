using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Controller
{

    public class LinuxController : IControllerBase
    {

        public StreamWriter input;
        public StreamReader erroutput;
        public string errBuffer = "";
        public Process xdotool;
        public StreamReader output;
        public Process xmodmap;
        public Dictionary<string, string> keyMap = new Dictionary<string, string>();

        public LinuxController()
        {
            errBuffer = "";
            xdotool = new Process();
            xmodmap = new Process();
            keyMap = new Dictionary<string, string>();

            CreateKeyCodesMapping();
            Execute();
        }

        public void Execute()
        {
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = "xdotool";

            psi.ArgumentList.Add("-");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardError = true;

            xdotool.StartInfo = psi;

            // error managment from xdotool
            xdotool.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data.Contains("No such key name"))
                {
                    // extract the key 
                    var match = Regex.Match(e.Data, @"'(.+?)'");

                    Console.WriteLine("xdotool coudn't find the key " + match.Groups[1].Value + ", you could try to add the keyname to its corresponding keycode on ~/.config/controllerKeyMap");

                    //theres a chance for duplicated errors using xdotool 

                    if (!errBuffer.Equals(e.Data))
                    {
                        // write the text for feedback (and emojis)

                        WriteText(match.Groups[1].Value);
                        errBuffer = e.Data;
                    } else
                    {
                        errBuffer = "";
                    }

                }
            };
            xdotool.Start();

            input = xdotool.StandardInput;

            xdotool.BeginErrorReadLine();
        }

        public void Close()
        {
            input.Close();
            xdotool.WaitForExit(500);
            xdotool.Close();
            xdotool.Dispose();

        }

        static int accelerationCounter = 0;
        static int aceleration = 5;
        static string lastInput = "";
        static string lastInputClick = "";

        public string getType()
        {
            return "LinuxController";
        }

        // not every pc keyboard layout is the same
        public void CreateKeyCodesMapping()
        {
            ProcessStartInfo psi = new ProcessStartInfo();

            var user = Environment.GetEnvironmentVariable("USER");

            psi.FileName = "/bin/bash";
            psi.ArgumentList.Add("-c");
            psi.ArgumentList.Add("xmodmap -pke > /home/" + user + "/.config/controllerKeyMap");

            xmodmap.StartInfo = psi;
            xmodmap.Start();

            // this is horrible but it works 
            while (!xmodmap.HasExited) { continue; }

            string[] file = File.ReadAllLines("/home/" + user + "/.config/controllerKeyMap");

            foreach (string line in file)
            {
                string[] collumns = Regex.Replace(line, @"\s+", " ").Split(" ");
                if (collumns.Length > 3)
                {
                    Console.WriteLine(Regex.Replace(line, @"\s+", " "));
                    if (!keyMap.ContainsKey(collumns[3].ToLower()))
                    {

                        keyMap.Add(collumns[3].ToLower(), collumns[1]);
                    }
                }

                //keyMap.Add(collumns[4], collumns[1]); 
            }
            Console.WriteLine(keyMap.ToString());
        }
        public void WriteTextSpecial(string text)
        {
            // instead of using a enum, we get a dictionary from controllerKeyMap file on ~/.config

            if (!keyMap.TryGetValue(text, out string keyValue))
            {
                Console.WriteLine("There is no keycode available for this input on the keyboard layout");

                // use xdotool mapping instead
                if (text.Equals("esc"))
                {
                    text = "Escape";
                }

                input.Write("key --clearmodifiers " + text + "\n");

            }
            else
            {
                input.Write("key --clearmodifiers " + keyValue + "\n");
            }


        }
        public void WriteText(string text)
        {
            // special case for space key and backspace key
            // TODO: there must be a better way of doing this

            if (text.Equals(" "))
            {
                input.Write("key space\n");
            }
            else {
                input.Write("type " + text + "\n");
            }
            input.Flush();
        }
        public void MoveMouse(string direction)
        {
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


            String xdotoolcmd = "mousemove_relative";

            if (direction.Equals("left"))
            {
                //List<String> commands = [xdotoolcmd, "--", (-aceleration).ToString(), "0"];
                //Execute(commands);
                input.Write(xdotoolcmd + " " + "--" + " " + -aceleration + " " + 0 + "\n");
            }
            else if (direction.Equals("right"))
            {
                //List<String> commands = [xdotoolcmd, aceleration.ToString(), "0"];
                //Execute(commands);
                input.Write(xdotoolcmd + " " + aceleration + " " + 0 + "\n");
            }
            else if (direction.Equals("down"))
            {
                //List<String> commands = [xdotoolcmd, "0",aceleration.ToString()];
                //Execute(commands);
                input.Write(xdotoolcmd + " " + 0 + " " + aceleration + "\n");
            }
            else if (direction.Equals("up"))
            {
                //List<String> commands = [xdotoolcmd, "0", (-aceleration).ToString()];
                input.Write(xdotoolcmd + " " + 0 + " " + -aceleration + "\n");
                //Execute(commands);
            }
            input.Flush();
        }


        public void ClickMouse(string button)
        {
            //GetCursorPos(out POINT pt);


            if (button.Equals("left_click"))
            {
                //mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)pt.x, (uint)pt.y, 0, 0);
                input.Write("mousedown 1\n");

            }
            else if (button.Equals("right_click"))
            {
                //mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint)pt.x, (uint)pt.y, 0, 0);
                input.Write("mousedown 3\n");
            }
            else if (button.Equals("right_click_up") && !lastInputClick.Equals("right_click_up"))
            {
                //mouse_event(MOUSEEVENTF_RIGHTUP, (uint)pt.x, (uint)pt.y, 0, 0);
                input.Write("mouseup 3\n");
            }
            else if (button.Equals("left_click_up") && !lastInputClick.Equals("left_click_up"))
            {
                input.Write("mouseup 1\n");
                //mouse_event(MOUSEEVENTF_LEFTUP, (uint)pt.x, (uint)pt.y, 0, 0);

            }
            else if (button.Contains("wheel_up"))
            {
                input.Write("click 4\n");
                //mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 50, 0);
            }
            else if (button.Contains("wheel_down"))
            {
                input.Write("click 5\n");
                //mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (unchecked((uint)(-120))), 0);
            }

            lastInputClick = button;
            input.Flush();
        }

        public void VolumenChange(string vol_dir)
        {
            // this bifurcation is redundant, but later on will make sense when direct volume control being implemented so idk :b
            if (vol_dir == "vol_up")
            {
                WriteTextSpecial("XF86AudioRaiseVolume");

            }
            else if (vol_dir == "vol_down")
            {
                WriteTextSpecial("XF86AudioLowerVolume");
            }
        }

        public void JoyStickMoveMouse(string coordinates)
        {
            

            // format of message: joystick:x:y

            float dx = (float) Double.Parse(coordinates.Split(":")[1]); 
            float dy = (float) Double.Parse(coordinates.Split(":")[2]); 

            float Xaceleration = 0.3f;
            float Yaceleration = 0.2f;


            String xdotoolcmd = "mousemove_relative";

            float vx = dx * Xaceleration;
            float vy = dy * Yaceleration;

            if (vx < 0)
            {
                input.Write(xdotoolcmd + " " + "--" + " " + vx + " " + vy + "\n");
            } else
            {
                input.Write(xdotoolcmd + " " + vx + " " + vy + "\n");
            }
            
        }

    }


}
