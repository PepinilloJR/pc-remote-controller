using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class WindowsController
    {
        // importacion de metodos del windows API

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);


    }


}
