
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
        
            Receiver receiver = new Receiver(cliente);

            string mensaje = receiver.listen();
        
        }

    }

}

