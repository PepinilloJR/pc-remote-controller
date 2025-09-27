namespace Controller
{
    public interface Controller
    {
        public string getType();

        public void MoveMouse(string direction);

        public void WriteText(string input);
    }

}