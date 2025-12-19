using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Controller
{

    public class LinuxController : Controller
    {

        public static StreamWriter input;
        public static StreamReader erroutput;
        public static Process xdotool;
        public static StreamReader output;
        public static Process xmodmap;
        public static Dictionary<string, string> keyMap = new Dictionary<string, string>();

        public static void Execute()
        {
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = "xdotool";
            //foreach (String command in args)
            //{
            //    psi.ArgumentList.Add(command);
            //}

            psi.ArgumentList.Add("-");
            psi.RedirectStandardInput = true;
            psi.RedirectStandardError = true;
        

            xdotool = new Process();
            xdotool.StartInfo = psi;

            // error managment from xdotool
            xdotool.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data.Contains("No such key name"))
                {
                    // extract the key 
                    
                    var match = Regex.Match(e.Data, @"'(.+?)'");

                    Console.WriteLine("xdotool coudn't find the key " + match.Groups[1].Value +", you could try to add the keyname to its corresponding keycode on ~/.config/controllerKeyMap");
                    
                    // write the text for feedback (and emojis)
                    WriteText(match.Groups[1].Value);
                } 
                //Console.WriteLine(e.Data);
            };
            xdotool.Start();

            input = xdotool.StandardInput;

            xdotool.BeginErrorReadLine();
        }

        static public void Close()
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
        public static void CreateKeyCodesMapping ()
        {
            ProcessStartInfo psi = new ProcessStartInfo();

            var user = Environment.GetEnvironmentVariable("USER");

            psi.FileName = "/bin/bash";
            psi.ArgumentList.Add("-c");
            psi.ArgumentList.Add("xmodmap -pke > /home/"+user+"/.config/controllerKeyMap");

            xmodmap = new Process();
            xmodmap.StartInfo = psi;
            xmodmap.Start();

            // this is horrible but it works 
            while (!xmodmap.HasExited) { continue;}

            string[] file = File.ReadAllLines("/home/"+user+"/.config/controllerKeyMap");

            foreach (string line in file)
            {
                string[] collumns = Regex.Replace(line, @"\s+", " ").Split(" ");
                if (collumns.Length > 3)
                {
                    Console.WriteLine(Regex.Replace(line, @"\s+", " "));
                    if (!keyMap.ContainsKey(collumns[3].ToLower())) {
                        
                        keyMap.Add(collumns[3].ToLower(), collumns[1]);
                    }
                }

                //keyMap.Add(collumns[4], collumns[1]); 
            }
            Console.WriteLine(keyMap.ToString());
        }
        public static void WriteTextSpecial(string text)
        {
            // instead of using a enum, we get a dictionary from controllerKeyMap file on ~/.config
            
            if (!keyMap.TryGetValue(text, out string keyValue))
            {
                Console.WriteLine("There is no keycode available for this input on the keyboard layout");

                // use xdotool mapping instead

                input.Write("key --clearmodifiers " + text + "\n");

            } else
            {
                input.Write("key --clearmodifiers " + keyValue + "\n");
            }
            

        } 
        public static void WriteText(string text)
        {
            // special case for space key and backspace key
            // there must be a better way of doing this
            if (text.Equals(" "))
            {
                input.Write("key space\n");
            } else
            {
                input.Write("type " + text + "\n");
            }
            input.Flush();
        }
        public static void MoveMouse(string direction)
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


        public static void ClickMouse(string button)
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

    }
}
