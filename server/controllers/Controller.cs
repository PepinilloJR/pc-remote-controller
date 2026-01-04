namespace Controller
{
    public interface IControllerBase
    {
        public string getType();

        public void MoveMouse(string direction);

        public void JoyStickMoveMouse(string coordinates) {}

        public void WriteText(string input);

        public void WriteTextSpecial(string input);

        public void ClickMouse(string input);

        public void VolumenChange(string input);
    }

}