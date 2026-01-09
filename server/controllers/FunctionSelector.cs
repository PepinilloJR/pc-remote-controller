namespace Controller
{
    public class FunctionSelector
    {
        static long lastMessageTimeStamp = 0;

        public static string parseMessage(string crude)
        {
            string[] messages = crude.TrimEnd('\r', '\n', '\0').Split('|');
            string message = messages[messages.Length - 1].ToLower();

            if (message.Split("#").Length > 1)
            {
                long currentTimeStamp = long.Parse(message.Split("#")[1]);

                if (currentTimeStamp < lastMessageTimeStamp)
                {
                    return "Ignore";
                }

                lastMessageTimeStamp = currentTimeStamp;
                message = message.Split("#")[0];
            }

            return message;
        }

        public static void selectFunction(string message_, IControllerBase controller)
        {

            string message = parseMessage(message_);

            Console.WriteLine("Received: " + message);

            if (message.Length == 1)
            {
                //Console.WriteLine(mensaje);
                controller.WriteText(message);
                //WindowsController.WriteText(mensaje.ToCharArray()[0]);
            }
            else if (message.Contains("special"))
            {
                message = message.Replace("special", "").ToLower();
                controller.WriteTextSpecial(message);
                //WindowsController.WriteTextSpecial(mensaje);
            }
            else if (message.Contains("vol"))
            {
                //WindowsController.VolumenChange(mensaje);
                controller.VolumenChange(message);
            }
            else if (message.Contains("joystick"))
            {
                controller.JoyStickMoveMouse(message);
            }
            else if (message.Contains("Ignore"))
            {

            }
            else
            {
                //WindowsController.MoveMouse(mensaje);
                //controller.MoveMouse(message);
                controller.ClickMouse(message);
                //WindowsController.ClickMouse(mensaje);
            }
        }
    }

}