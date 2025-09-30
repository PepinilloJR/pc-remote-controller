
using Controllers;
using Reception;
using ServerController;
using System.Net.Sockets;


namespace main
{
    class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server(8000);

            Socket cliente = server.createSocket().bind().listen().awaitConnection();

            Console.WriteLine("conexion iniciada");

            Receiver receiver = new Receiver(cliente);


            while (true)
            {
                string mensaje = receiver.listen();
                WindowsController.MoveMouse(mensaje);
                WindowsController.ClickMouse(mensaje);
                if (mensaje.Length == 1)
                {
                    Console.WriteLine(mensaje);
                    WindowsController.WriteText(mensaje.ToCharArray()[0]);
                }

            }
        

        }

    }

}

