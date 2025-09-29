namespace Controller
{
    public interface Controller
    {
        public string getType();

        public static void MoveMouse(string direction) { }

        public static void WriteText(string input) { }

        public static void ClickMouse(string input) { }
    }

}