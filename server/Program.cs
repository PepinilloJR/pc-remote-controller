
using Controller;
using Reception;
using ServerController;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;


namespace main
{
    class Program
    {
        public static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Welcome to PC-remote-controller.");

                Console.WriteLine("1. Start service");
                Console.WriteLine("2. Start service with non-default IP and PORT");
                Console.WriteLine("");
                Console.Write("->");
                String op = Console.ReadLine();

                Server server;
                if (op.Equals("1")) {
                    server = new Server(8000);
                } else if (op.Equals("2")) {
                    Console.WriteLine("");
                    Console.Write("IP ->");
                    String ip = Console.ReadLine();
                    Console.WriteLine("");
                    Console.Write("Port ->");
                    String port = Console.ReadLine();

                    try
                    {
                        server = new Server(ip, int.Parse(port));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The IP or PORT specified is not valid or not available");
                        Console.WriteLine("There's something wrong with the format provided or the port is being used by another service");
                        continue;
                    }

                } else { continue; }


                Socket cliente = server.createSocket().bind().listen().awaitConnection();

                Console.WriteLine("conexion iniciada");

                Receiver receiver = new Receiver(cliente);

                CancellationTokenSource tokenSource = new CancellationTokenSource();

                CancellationToken token = tokenSource.Token;

                IControllerBase controller;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    controller = new LinuxController();
                } else
                {
                    controller = new WindowsController();
                }
                

                Task recibir = server.Receive(controller, receiver, tokenSource,token);

                try {
                    Task.WaitAll(recibir);
                    server.Close(); 
                    cliente.Close();
                    cliente.Dispose();  
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Posible desconexion repentina del cliente");
                    server.Close();
                    cliente.Close();
                    cliente.Dispose();
                }

                
            }
        }

    }

}

